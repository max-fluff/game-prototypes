using UnityEngine;

namespace MaxFluff.Prototypes
{
    public class PlayerCameraBinding : IInitBinding, IRunBinding
    {
        private readonly CameraPresenter _mainCamera;
        private readonly PlatformerPlayerPresenter _playerPresenter;

        private float _zOffset;

        public PlayerCameraBinding(
            CameraPresenter mainCamera,
            PlatformerPlayerPresenter playerPresenter)
        {
            _mainCamera = mainCamera;
            _playerPresenter = playerPresenter;
        }

        public void Init()
        {
            _zOffset = _mainCamera.Transform.position.z - _playerPresenter.Transform.position.z;
        }

        public void Run()
        {
            var playerPosition = _playerPresenter.Transform.position;
            //var velocityMagnitude = _playerPresenter.Rigidbody.velocity.magnitude;
            //var offsetMultiplier = velocityMagnitude > 10f ? velocityMagnitude / 10f : 1f;
            playerPosition.z += _zOffset;
            _mainCamera.Transform.position = playerPosition;

            _mainCamera.Transform.rotation = _playerPresenter.State == PlatformerPlayerState.Triangle
                ? _playerPresenter.Transform.rotation
                : Quaternion.identity;
        }
    }
}