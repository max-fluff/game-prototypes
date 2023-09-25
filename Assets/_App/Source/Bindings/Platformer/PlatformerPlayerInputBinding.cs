using System;
using UnityEngine;

namespace MaxFluff.Prototypes
{
    public class PlatformerPlayerInputBinding : IInitBinding, IRunBinding, IDestroyBinding
    {
        private readonly PlatformerPlayerPresenter _player;
        private readonly StateBasedGameObjectsControllerPresenter _stateBasedGameObjectsController;
        private readonly StateSwitchAbilityTriggersListPresenter _stateSwitchAbilityTriggersList;
        private readonly KeyboardInputService _keyboardInput;
        private readonly RaycastPresenter _raycastPresenter;
        private readonly GravityService _gravityService;

        private bool _additionalJumpLeft;
        private bool _switchAvailable = true;
        private int _defaultLayer;
        private int _ignoreLightLayer;

        private int floorLayer => _defaultLayer | _ignoreLightLayer;

        private bool IsPlayerOnFloor
        {
            get
            {
                var downVector = _player.State == PlatformerPlayerState.Circle ? Vector3.down : -_player.Transform.up;
                var rightVector = _player.State == PlatformerPlayerState.Circle
                    ? Vector3.right
                    : _player.Transform.right;
                return _raycastPresenter.PhysicsRaycast(new Ray(_player.Transform.position, downVector),
                           out _, 1,
                           floorLayer) ||
                       _raycastPresenter.PhysicsRaycast(
                           new Ray(_player.Transform.position + rightVector * 0.5f, downVector),
                           out _, 1, floorLayer) ||
                       _raycastPresenter.PhysicsRaycast(
                           new Ray(_player.Transform.position - rightVector * 0.5f, downVector),
                           out _, 1, floorLayer);
            }
        }

        public PlatformerPlayerInputBinding(
            PlatformerPlayerPresenter player,
            StateBasedGameObjectsControllerPresenter stateBasedGameObjectsController,
            StateSwitchAbilityTriggersListPresenter stateSwitchAbilityTriggersList,
            KeyboardInputService keyboardInput,
            RaycastPresenter raycastPresenter,
            GravityService gravityService)
        {
            _player = player;
            _stateBasedGameObjectsController = stateBasedGameObjectsController;
            _stateSwitchAbilityTriggersList = stateSwitchAbilityTriggersList;
            _keyboardInput = keyboardInput;
            _raycastPresenter = raycastPresenter;
            _gravityService = gravityService;
        }

        public void Init()
        {
            _keyboardInput.OnInputAction += ProcessInputAction;
            _stateSwitchAbilityTriggersList.OnSetStateSwitchAbilityState += SetSwitchAbilityState;

            SetState(PlatformerPlayerState.Square);

            _gravityService.GravityPower *= 2;

            _defaultLayer = LayerMask.GetMask("Default");
            _ignoreLightLayer = LayerMask.GetMask("IgnoreLight");
        }

        private void SetSwitchAbilityState(bool isAvailable) =>
            _switchAvailable = isAvailable;

        private void ProcessInputAction(Actions action)
        {
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
                _player.Transform.position += _player.Transform.up * 0.1f;
                _gravityService.SetGravityDirection(direction);
                var angle = Vector3.SignedAngle(-direction, _player.Transform.up, Vector3.forward);
                _player.Transform.Rotate(Vector3.forward, -angle);
            }
        }

        private void SetState(PlatformerPlayerState state)
        {
            if (!_switchAvailable || state == _player.State || _player.Energy < PlatformerPlayerPresenter.MaxEnergy) return;

            _player.Energy = 0;

            SetGravityDirection(Vector3.down, true);

            _player.State = state;
            _stateBasedGameObjectsController.SetState(state);
        }

        private void ProcessJumpAction()
        {
            switch (_player.State)
            {
                case PlatformerPlayerState.Square:
                    if (IsPlayerOnFloor)
                    {
                        _player.Rigidbody.AddForce(180 * Vector2.up);
                        _additionalJumpLeft = true;
                    }
                    else
                    {
                        if (_additionalJumpLeft)
                        {
                            _player.Rigidbody.AddForce(180 * Vector2.up);
                            _additionalJumpLeft = false;
                        }
                    }

                    break;
                case PlatformerPlayerState.Circle:
                    if (IsPlayerOnFloor && _player.Rigidbody.velocity.y < 0.01f)
                        _player.Rigidbody.AddForce(400 * 20 * Vector2.up);
                    break;
            }
        }

        public void Run()
        {
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

                    _player.Rigidbody.AddForce(resultingHorizontalMovement * 50 * 20 * Vector2.right);
                    if (_player.Rigidbody.velocity.magnitude > 40)
                        _player.Rigidbody.velocity = _player.Rigidbody.velocity.normalized * 30;

                    break;
            }
        }

        public void Destroy()
        {
            _gravityService.ResetGravity();
        }
    }
}