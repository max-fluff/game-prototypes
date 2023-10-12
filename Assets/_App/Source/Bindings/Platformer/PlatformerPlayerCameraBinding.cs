using UnityEngine;

namespace MaxFluff.Prototypes
{
    public class PlatformerPlayerCameraBinding : IInitBinding, IRunBinding
    {
        private readonly CameraPresenter _mainCamera;
        private readonly PlatformerPlayerPresenter _playerPresenter;

        private Vector3 _offset;

        public PlatformerPlayerCameraBinding(
            CameraPresenter mainCamera,
            PlatformerPlayerPresenter playerPresenter)
        {
            _mainCamera = mainCamera;
            _playerPresenter = playerPresenter;
        }

        public void Init()
        {
            _offset = _mainCamera.Transform.position - _playerPresenter.Transform.position;
        }

        public void Run()
        {
            var playerPosition = _playerPresenter.Transform.position;
            //var velocityMagnitude = _playerPresenter.Rigidbody.velocity.magnitude;
            //var offsetMultiplier = velocityMagnitude > 10f ? velocityMagnitude / 10f : 1f;
            playerPosition += _offset;
            _mainCamera.Transform.position = playerPosition;

            _mainCamera.Transform.rotation = _playerPresenter.State == PlatformerPlayerState.Triangle
                ? _playerPresenter.Transform.rotation
                : Quaternion.identity;
        }
    }
}