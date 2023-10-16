using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace MaxFluff.Prototypes
{
    public class PlatformerPlayerInputBinding : IInitBinding, IRunBinding, IDestroyBinding
    {
        private readonly PlatformerPlayerPresenter _player;
        private readonly StateBasedGameObjectsControllerPresenter _stateBasedGameObjectsController;
        private readonly StateSwitchAbilityTriggersListPresenter _stateSwitchAbilityTriggersList;
        private readonly WindowsOrganizerPresenter _windowsOrganizerPresenter;
        private readonly KeyboardInputService _keyboardInput;
        private readonly RaycastPresenter _raycastPresenter;
        private readonly GravityService _gravityService;

        private bool _additionalJumpLeft;
        private bool _switchAvailable = true;
        private int _defaultLayer;
        private int _ignoreLightLayer;

        private float _timeTillLastOnFloor;
        private const float COYOTE_TIME = 0.1f;
        private bool _isJumping = false;

        private int floorLayer => _defaultLayer | _ignoreLightLayer;

        private bool IsPlayerOnFloor
        {
            get
            {
                var downVector = _player.State == PlatformerPlayerState.Circle ? Vector3.down : -_player.Transform.up;
                const float length = 1.2f;
                var sideLength = _player.State == PlatformerPlayerState.Circle ? 1.2f : 1.1f;
                var rightVector = _player.State == PlatformerPlayerState.Circle
                    ? Vector3.right
                    : _player.Transform.right;
                return _raycastPresenter.PhysicsRaycast(new Ray(_player.Transform.position, downVector),
                           out _, length, floorLayer) ||
                       _raycastPresenter.PhysicsRaycast(
                           new Ray(_player.Transform.position + rightVector * 0.5f, downVector),
                           out _, sideLength, floorLayer) ||
                       _raycastPresenter.PhysicsRaycast(
                           new Ray(_player.Transform.position - rightVector * 0.5f, downVector),
                           out _, sideLength, floorLayer);
            }
        }

        public PlatformerPlayerInputBinding(
            PlatformerPlayerPresenter player,
            StateBasedGameObjectsControllerPresenter stateBasedGameObjectsController,
            StateSwitchAbilityTriggersListPresenter stateSwitchAbilityTriggersList,
            WindowsOrganizerPresenter windowsOrganizerPresenter,
            KeyboardInputService keyboardInput,
            RaycastPresenter raycastPresenter,
            GravityService gravityService)
        {
            _player = player;
            _stateBasedGameObjectsController = stateBasedGameObjectsController;
            _stateSwitchAbilityTriggersList = stateSwitchAbilityTriggersList;
            _windowsOrganizerPresenter = windowsOrganizerPresenter;
            _keyboardInput = keyboardInput;
            _raycastPresenter = raycastPresenter;
            _gravityService = gravityService;
        }

        private async UniTask KeepConstraintsUntilFloorTouch()
        {
            var cachedGravity = Physics.gravity;

            if (Mathf.Abs(Physics.gravity.y) > 0.01f)
                _player.Rigidbody.constraints |= RigidbodyConstraints.FreezePositionX;

            if (Mathf.Abs(Physics.gravity.x) > 0.01f)
                _player.Rigidbody.constraints |= RigidbodyConstraints.FreezePositionY;

            await UniTask.WaitUntil(() => IsPlayerOnFloor || cachedGravity != Physics.gravity);

            _player.Rigidbody.constraints &= ~RigidbodyConstraints.FreezePositionX;
            _player.Rigidbody.constraints &= ~RigidbodyConstraints.FreezePositionY;
        }


        public void Init()
        {
            _keyboardInput.OnInputAction += ProcessInputAction;
            _stateSwitchAbilityTriggersList.OnSetStateSwitchAbilityState += SetSwitchAbilityState;

            _stateBasedGameObjectsController.SetState(PlatformerPlayerState.None);

            _gravityService.GravityPower *= 2;

            _defaultLayer = LayerMask.GetMask("Default");
            _ignoreLightLayer = LayerMask.GetMask("IgnoreLight");
        }

        private void SetSwitchAbilityState(bool isAvailable) =>
            _switchAvailable = isAvailable;

        private void ProcessInputAction(Actions action)
        {
            if (_windowsOrganizerPresenter.IsAnyWindowOpened)
                return;

            switch (action)
            {
                case Actions.Up:
                    if (_player.State == PlatformerPlayerState.Triangle)
                        SetGravityDirection(_player.Transform.up);
                    else
                        ProcessJumpAction();
                    break;
                case Actions.Down:
                    if (_player.State == PlatformerPlayerState.Triangle)
                        SetGravityDirection(-_player.Transform.up);
                    break;
                case Actions.Left:
                    if (_player.State == PlatformerPlayerState.Triangle)
                        SetGravityDirection(-_player.Transform.right);
                    break;
                case Actions.Right:
                    if (_player.State == PlatformerPlayerState.Triangle)
                        SetGravityDirection(_player.Transform.right);
                    break;
                case Actions.Selection1:
                    SetState(PlatformerPlayerState.Square);
                    break;
                case Actions.Selection2:
                    SetState(PlatformerPlayerState.Circle);
                    break;
                case Actions.Selection3:
                    SetState(PlatformerPlayerState.Triangle);
                    break;
            }
        }

        private void SetGravityDirection(Vector3 direction, bool forced = false)
        {
            if (IsPlayerOnFloor || forced)
            {
                var wasSet = _gravityService.SetGravityDirection(direction);

                if (wasSet)
                {
                    _player.PlayJumpSound();

                    _player.Transform.position += _player.Transform.up * 0.3f;
                    var angle = Vector3.SignedAngle(-direction, _player.Transform.up, Vector3.forward);
                    _player.Transform.Rotate(Vector3.forward, -angle);

                    KeepConstraintsUntilFloorTouch().Forget();
                }
            }
        }

        private void SetState(PlatformerPlayerState state)
        {
            const int energyToUse = PlatformerPlayerPresenter.MaxEnergy / 2;

            if (!_switchAvailable || state == _player.State ||
                _player.Energy < energyToUse) return;

            _player.Energy -= energyToUse;

            _player.State = state;
            _player.PlayStateSound();
            _stateBasedGameObjectsController.SetState(state);

            SetGravityDirection(Vector3.down, true);
        }

        private void MakeJump(float value)
        {
            if (_isJumping) return;

            _player.PlayJumpSound();

            var velocity = _player.Rigidbody.velocity;
            if (velocity.y < 0f)
                velocity.y = 0;
            else
                velocity.y /= 2;

            _player.Rigidbody.velocity = velocity;

            JumpAsync(value).Forget();
        }

        private async UniTask JumpAsync(float value)
        {
            var count = 0;
            const int division = 5;
            const int jumpDuration = 60;

            while (count < division && _keyboardInput.IsActionHeld(Actions.Up))
            {
                _isJumping = true;
                _player.Rigidbody.AddForce(value / division * Vector2.up);
                count++;
                await UniTask.Delay(jumpDuration / division, DelayType.Realtime);
            }

            _isJumping = false;
        }

        private void ProcessJumpAction()
        {
            switch (_player.State)
            {
                case PlatformerPlayerState.Square:
                    if (IsPlayerOnFloor || _timeTillLastOnFloor < COYOTE_TIME)
                    {
                        MakeJump(180F);
                        _additionalJumpLeft = true;
                    }
                    else
                    {
                        if (_additionalJumpLeft)
                        {
                            _player.PlayJumpSound();

                            MakeJump(180F);

                            _additionalJumpLeft = false;
                        }
                    }

                    break;
                case PlatformerPlayerState.Circle:
                    if (IsPlayerOnFloor || _timeTillLastOnFloor < COYOTE_TIME)
                    {
                        _player.PlayJumpSound();

                        MakeJump(8000F);
                    }

                    break;
            }
        }

        public void Run()
        {
            var isOnFloor = IsPlayerOnFloor;

            if (isOnFloor)
                _timeTillLastOnFloor = 0;
            else
                _timeTillLastOnFloor += Time.deltaTime;

            if (!_additionalJumpLeft && isOnFloor)
                _additionalJumpLeft = true;

            if (_windowsOrganizerPresenter.IsAnyWindowOpened)
                return;

            var resultingHorizontalMovement =
                (_keyboardInput.IsActionHeld(Actions.Left) ? -1 : 0) +
                (_keyboardInput.IsActionHeld(Actions.Right) ? 1 : 0);

            switch (_player.State)
            {
                case PlatformerPlayerState.Square:

                    var velocity = _player.Rigidbody.velocity;
                    velocity.x = resultingHorizontalMovement * 10;
                    _player.Rigidbody.velocity = velocity;

                    break;
                case PlatformerPlayerState.Circle:

                    _player.Rigidbody.AddForce(Vector2.right * (resultingHorizontalMovement * 30000 * Time.deltaTime));
                    if (_player.Rigidbody.velocity.magnitude > 40)
                        _player.Rigidbody.velocity = _player.Rigidbody.velocity.normalized * 30;

                    break;
            }
        }

        public void Destroy()
        {
            _gravityService.ResetGravity();
            _keyboardInput.OnInputAction -= ProcessInputAction;
        }
    }
}