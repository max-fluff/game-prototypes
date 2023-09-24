using System;
using UnityEngine;

namespace MaxFluff.Prototypes
{
    public class PlatformerPlayerInputBinding : IInitBinding, IRunBinding, IDestroyBinding
    {
        private readonly PlatformerPlayerPresenter _player;
        private readonly KeyboardInputService _keyboardInput;

        private int _cachedHorizontalInput;

        public PlatformerPlayerInputBinding(
            PlatformerPlayerPresenter player,
            KeyboardInputService keyboardInput)
        {
            _player = player;
            _keyboardInput = keyboardInput;
        }

        public void Init()
        {
            _keyboardInput.OnInputAction += ProcessInputAction;

            _player.State = PlatformerPlayerState.Square;

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
                case Actions.Selection1:
                    _player.State = PlatformerPlayerState.Square;
                    break;
                case Actions.Selection2:
                    _player.State = PlatformerPlayerState.Circle;
                    break;
                case Actions.Selection3:
                    break;
            }
        }

        private void ProcessJumpAction()
        {
            switch (_player.State)
            {
                case PlatformerPlayerState.Square:
                    _player.Rigidbody.AddForce(200 * Vector2.up);
                    break;
                case PlatformerPlayerState.Circle:
                    _player.Rigidbody.AddForce(400 * Vector2.up);
                    break;
                case PlatformerPlayerState.Triangle:
                    break;
            }
        }

        public void Run()
        {
            var resultingHorizontalMovement =
                (_keyboardInput.IsActionDown(Actions.Left) ? -1 : 0) +
                (_keyboardInput.IsActionDown(Actions.Right) ? 1 : 0);
            
            switch (_player.State)
            {
                case PlatformerPlayerState.Square:

                    var velocity = _player.Rigidbody.velocity;
                    velocity.x = resultingHorizontalMovement * 20;
                    _player.Rigidbody.velocity = velocity;

                    break;
                case PlatformerPlayerState.Circle:

                    _player.Rigidbody.AddForce(resultingHorizontalMovement * 50 * Vector2.right);
                    if (_player.Rigidbody.velocity.magnitude > 45)
                        _player.Rigidbody.velocity = _player.Rigidbody.velocity.normalized * 30;

                    break;
                case PlatformerPlayerState.Triangle:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            _cachedHorizontalInput = resultingHorizontalMovement;
        }

        public void Destroy()
        {
            Physics.gravity /= 2;
        }
    }
}