﻿using Cysharp.Threading.Tasks;
using MaxFluff.Prototypes.FPS;
using UnityEngine;

namespace MaxFluff.Prototypes
{
    public class FPSPlayerInputBinding : IInitBinding, IRunBinding, IDestroyBinding
    {
        private readonly FPSPlayerPresenter _player;
        private readonly CameraPresenter _mainCamera;
        private readonly WindowsOrganizerPresenter _windowsOrganizerPresenter;
        private readonly KeyboardInputService _keyboardInput;
        private readonly MouseInputService _mouseInputService;
        private readonly RaycastPresenter _raycastPresenter;
        private readonly GravityService _gravityService;
        private readonly CursorService _cursorService;

        private int _defaultLayer;
        private int _ignoreLightLayer;

        private bool _isJumping;

        public FPSPlayerInputBinding(
            FPSPlayerPresenter player,
            CameraPresenter mainCamera,
            WindowsOrganizerPresenter windowsOrganizerPresenter,
            RaycastPresenter raycastPresenter,
            KeyboardInputService keyboardInput,
            MouseInputService mouseInputService,
            GravityService gravityService,
            CursorService cursorService)
        {
            _player = player;
            _mainCamera = mainCamera;
            _windowsOrganizerPresenter = windowsOrganizerPresenter;
            _keyboardInput = keyboardInput;
            _mouseInputService = mouseInputService;
            _raycastPresenter = raycastPresenter;
            _gravityService = gravityService;
            _cursorService = cursorService;
        }

        private int floorLayer => _defaultLayer | _ignoreLightLayer;

        private bool IsPlayerOnFloor
        {
            get
            {
                var downVector = -_player.Transform.up;
                const float length = 0.5f;
                var rightVector = _player.Transform.right;
                return _raycastPresenter.PhysicsRaycast(
                           new Ray(_player.Transform.position + Vector3.up * 0.1f, downVector),
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

            _gravityService.GravityPower *= 15;

            _defaultLayer = LayerMask.GetMask("Default");
            _ignoreLightLayer = LayerMask.GetMask("IgnoreLight");

            _cursorService.IsCursorVisible = false;
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
                MakeJump(24000F);
            }
        }

        public void Run()
        {
            if (_windowsOrganizerPresenter.IsAnyWindowOpened)
                return;

            var resultingHorizontalMovement =
                (_keyboardInput.IsActionHeld(Actions.Left) ? -1 : 0) +
                (_keyboardInput.IsActionHeld(Actions.Right) ? 1 : 0);

            var resultingVerticalMovement =
                (_keyboardInput.IsActionHeld(Actions.Down) ? -1 : 0) +
                (_keyboardInput.IsActionHeld(Actions.Up) ? 1 : 0);

            _player.Rigidbody.AddForce(_player.Transform.right *
                                       (resultingHorizontalMovement * 60000 * Time.deltaTime));
            _player.Rigidbody.AddForce(_player.Transform.forward *
                                       (resultingVerticalMovement * 60000 * Time.deltaTime));

            var velocity = _player.Rigidbody.velocity;
            var horizontalVelocity = new Vector2(velocity.x, velocity.z);
            var verticalVelocity = velocity.y;

            if (horizontalVelocity.magnitude > 80)
                horizontalVelocity = horizontalVelocity.normalized * 80;

            if (Mathf.Abs(verticalVelocity) > 200)
                verticalVelocity = Mathf.Abs(verticalVelocity) * Mathf.Sign(verticalVelocity);

            _player.Rigidbody.velocity =
                new Vector3(horizontalVelocity.x, verticalVelocity, horizontalVelocity.y);

            _player.Transform.Rotate(0f, _mouseInputService.MouseDelta.x / 5f, 0f);
            var transformRotation = _mainCamera.Transform.localRotation.eulerAngles +
                                    new Vector3(-_mouseInputService.MouseDelta.y / 5f, 0f, 0f);

            transformRotation.x = transformRotation.x > 180
                ? Mathf.Clamp(transformRotation.x, 300, 361)
                : Mathf.Clamp(transformRotation.x, -1, 60);
            
            _mainCamera.Transform.localRotation = Quaternion.Euler(transformRotation);
        }

        public void Destroy()
        {
            _gravityService.ResetGravity();
            _keyboardInput.OnInputAction -= ProcessInputAction;
            _cursorService.IsCursorVisible = true;
        }
    }
}