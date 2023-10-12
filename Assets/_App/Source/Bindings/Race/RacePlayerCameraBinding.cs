using UnityEngine;

namespace MaxFluff.Prototypes
{
    public class RacePlayerCameraBinding : IInitBinding, IRunBinding
    {
        private readonly CameraPresenter _mainCamera;
        private readonly RacePlayerPresenter _playerPresenter;

        private Vector3 _offset;

        public RacePlayerCameraBinding(
            CameraPresenter mainCamera,
            RacePlayerPresenter playerPresenter)
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
            /*var playerPosition = _playerPresenter.Transform.position;
            playerPosition += _playerPresenter.Transform.rotation * _offset;
            _mainCamera.Transform.position = playerPosition;
            
            _mainCamera.Transform.LookAt(_playerPresenter.Transform.position);*/
        }
    }
}