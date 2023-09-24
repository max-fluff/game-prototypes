using System;
using UnityEngine;

namespace MaxFluff.Prototypes
{
    public class PlatformerPlayerInputBinding : IInitBinding, IRunBinding, IDestroyBinding
    {
        private readonly PlatformerPlayerPresenter _player;
        private readonly StateBasedGameObjectsControllerPresenter _stateBasedGameObjectsController;
        private readonly KeyboardInputService _keyboardInput;
        private readonly RaycastPresenter _raycastPresenter;

        private bool _additionalJumpLeft;

        private bool IsPlayerOnFloor =>
            _raycastPresenter.DefaultRaycast(new Ray(_player.Transform.position, Vector3.down), out _, 1) ||
            _raycastPresenter.DefaultRaycast(new Ray(_player.Transform.position + Vector3.left * 0.5f, Vector3.down),
                out _, 1) ||
            _raycastPresenter.DefaultRaycast(new Ray(_player.Transform.position + Vector3.right * 0.5f, Vector3.down),
                out _, 1);

        public PlatformerPlayerInputBinding(
            PlatformerPlayerPresenter player,
            StateBasedGameObjectsControllerPresenter stateBasedGameObjectsController,
            KeyboardInputService keyboardInput,
            RaycastPresenter raycastPresenter)
        {
            _player = player;
            _stateBasedGameObjectsController = stateBasedGameObjectsController;
            _keyboardInput = keyboardInput;
            _raycastPresenter = raycastPresenter;
        }

        public void Init()
        {
            _keyboardInput.OnInputAction += ProcessInputAction;

            SetState(PlatformerPlayerState.Square);

            Physics.gravity *= 2;
        }

        private void ProcessInputAction(Actions action)
        {
            switch (action)
            {
                case Actions.Up:
                    ProcessJumpAction();
                    break;
                case Actions.Down:
                    break;
                case Actions.Left:
                    break;
                case Actions.Right:
                    break;
                case Actions.Selection1:
                    //if (IsPlayerOnFloor)
                    SetState(PlatformerPlayerState.Square);
                    break;
                case Actions.Selection2:
                    //if (IsPlayerOnFloor)
                    SetState(PlatformerPlayerState.Circle);
                    break;
                case Actions.Selection3:
                    break;
            }
        }

        private void SetState(PlatformerPlayerState state)
        {
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
                case PlatformerPlayerState.Triangle:
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
                case PlatformerPlayerState.Triangle:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public void Destroy()
        {
            Physics.gravity /= 2;
        }
    }
}