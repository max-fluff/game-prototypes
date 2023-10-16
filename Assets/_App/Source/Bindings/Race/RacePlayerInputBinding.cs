using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace MaxFluff.Prototypes
{
    public class RacePlayerInputBinding : IInitBinding, IRunBinding, IDestroyBinding
    {
        private readonly RacePlayerPresenter _player;
        private readonly WindowsOrganizerPresenter _windowsOrganizerPresenter;
        private readonly KeyboardInputService _keyboardInput;
        private readonly GravityChangeVisualizerPresenter _gravityChangeVisualizerPresenter;
        private readonly RaycastPresenter _raycastPresenter;
        private readonly GravityService _gravityService;

        private int _defaultLayer;
        private int _ignoreLightLayer;

        private const float RotationResetTime = 0.5f;
        private float _rotationTimer;
        private bool _wasOnFloorSinceLastRotation;

        private bool _isJumping;

        public RacePlayerInputBinding(
            RacePlayerPresenter player,
            WindowsOrganizerPresenter windowsOrganizerPresenter,
            KeyboardInputService keyboardInput,
            GravityChangeVisualizerPresenter gravityChangeVisualizerPresenter,
            RaycastPresenter raycastPresenter,
            GravityService gravityService)
        {
            _player = player;
            _windowsOrganizerPresenter = windowsOrganizerPresenter;
            _keyboardInput = keyboardInput;
            _gravityChangeVisualizerPresenter = gravityChangeVisualizerPresenter;
            _raycastPresenter = raycastPresenter;
            _gravityService = gravityService;
        }

        private int floorLayer => _defaultLayer | _ignoreLightLayer;

        private bool IsPlayerOnFloor
        {
            get
            {
                var downVector = -_player.Transform.up;
                const float length = 1.6f;
                var rightVector = _player.Transform.right;
                return _raycastPresenter.PhysicsRaycast(new Ray(_player.Transform.position, downVector),
                           out _, length, floorLayer) ||
                       _raycastPresenter.PhysicsRaycast(
                           new Ray(_player.Transform.position + rightVector * 0.5f, downVector),
                           out _, length, floorLayer) ||
                       _raycastPresenter.PhysicsRaycast(
                           new Ray(_player.Transform.position - rightVector * 0.5f, downVector),
                           out _, length, floorLayer);
            }
        }


        public void Init()
        {
            _keyboardInput.OnInputAction += ProcessInputAction;

            _gravityService.GravityPower *= 3;

            _defaultLayer = LayerMask.GetMask("Default");
            _ignoreLightLayer = LayerMask.GetMask("IgnoreLight");
        }

        private void ProcessInputAction(Actions action)
        {
            if (_windowsOrganizerPresenter.IsAnyWindowOpened)
                return;

            switch (action)
            {
                case Actions.Space:
                    ProcessJumpAction();
                    break;
                case Actions.Shift:
                    ProcessGravityChangeAction();
                    break;
            }
        }

        private void ProcessGravityChangeAction()
        {
            if (_rotationTimer > 0) return;
            
            var resultingHorizontal =
                (_keyboardInput.IsActionHeld(Actions.Left) ? -1 : 0) +
                (_keyboardInput.IsActionHeld(Actions.Right) ? 1 : 0);

            var direction = _player.Transform.right * resultingHorizontal;

            if (resultingHorizontal != 0)
            {
                _gravityService.SetGravityDirection(direction);
                var angle = Vector3.SignedAngle(-direction, _player.Transform.up, Vector3.forward);
                var playerRotation = _player.Transform.rotation.eulerAngles;
                playerRotation.z -= angle;
                _player.Transform.DORotateQuaternion(Quaternion.Euler(playerRotation), 0.5f);
                
                _wasOnFloorSinceLastRotation = false;
                _rotationTimer = RotationResetTime;
                _gravityChangeVisualizerPresenter.SetActive(false);
            }
        }

        private void MakeJump(float value)
        {
            if (_isJumping) return;

            var velocity = _player.Rigidbody.velocity;
            if (Mathf.Abs(velocity.y) < 0f)
                velocity.y = 0;

            if (Mathf.Abs(velocity.x) < 0f)
                velocity.x = 0;

            _player.Rigidbody.velocity = velocity;

            JumpAsync(value).Forget();
        }

        private async UniTask JumpAsync(float value)
        {
            var count = 0;
            const int division = 5;
            const int jumpDuration = 60;

            while (count < division && _keyboardInput.IsActionHeld(Actions.Space))
            {
                _isJumping = true;
                _player.Rigidbody.AddForce(value / division * _player.Transform.up);
                count++;
                await UniTask.Delay(jumpDuration / division, DelayType.Realtime);
            }

            _isJumping = false;
        }

        private void ProcessJumpAction()
        {
            if (IsPlayerOnFloor)
            {
                MakeJump(18000F);
            }
        }

        public void Run()
        {
            if (!_wasOnFloorSinceLastRotation && _rotationTimer > 0 && IsPlayerOnFloor)
                _wasOnFloorSinceLastRotation = true;
            
            if (_windowsOrganizerPresenter.IsAnyWindowOpened)
                return;

            if (_wasOnFloorSinceLastRotation && _rotationTimer > 0)
            {
                _rotationTimer -= Time.deltaTime;
                if(_rotationTimer <= 0)
                    _gravityChangeVisualizerPresenter.SetActive(true);
            }

            var resultingHorizontalMovement =
                (_keyboardInput.IsActionHeld(Actions.Left) ? -1 : 0) +
                (_keyboardInput.IsActionHeld(Actions.Right) ? 1 : 0);

            var resultingVerticalMovement =
                (_keyboardInput.IsActionHeld(Actions.Down) ? -1 : 0) +
                (_keyboardInput.IsActionHeld(Actions.Up) ? 1 : 0);

            _player.Rigidbody.AddForce(_player.Transform.right *
                                       (resultingHorizontalMovement * 60000 * Time.deltaTime));
            _player.Rigidbody.AddForce(_player.Transform.forward *
                                       (resultingVerticalMovement * 80000 * Time.deltaTime));
            if (_player.Rigidbody.velocity.magnitude > 80)
                _player.Rigidbody.velocity = _player.Rigidbody.velocity.normalized * 80;
        }

        public void Destroy()
        {
            _gravityService.ResetGravity();
            _keyboardInput.OnInputAction -= ProcessInputAction;
        }
    }
}