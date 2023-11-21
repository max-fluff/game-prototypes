using UnityEngine;

namespace MaxFluff.Prototypes
{
    public class NarrativeBinding : IInitBinding, IRunBinding
    {
        private readonly PenPresenter _penPresenter;
        private readonly StampPresenter _stampPresenter;
        private readonly RaycastPresenter _raycastPresenter;
        private readonly CameraPresenter _cameraPresenter;
        private readonly MouseInputService _mouseInputService;

        private IDraggableObjectPresenter _currentDrag;
        private bool _freshlyPressed = false;

        public NarrativeBinding(
            PenPresenter penPresenter,
            StampPresenter stampPresenter,
            RaycastPresenter raycastPresenter,
            CameraPresenter cameraPresenter,
            MouseInputService mouseInputService)
        {
            _penPresenter = penPresenter;
            _stampPresenter = stampPresenter;
            _raycastPresenter = raycastPresenter;
            _cameraPresenter = cameraPresenter;
            _mouseInputService = mouseInputService;
        }

        public void Init()
        {
            _penPresenter.OnStartDrag += OnDragStarted;
            _stampPresenter.OnStartDrag += OnDragStarted;
        }

        private void OnDragStarted(IDraggableObjectPresenter newDrag)
        {
            _currentDrag?.Reset();
            _currentDrag = newDrag;
            _freshlyPressed = true;
        }

        public void Run()
        {
            if (_currentDrag != null)
            {
                var mousePos = _mouseInputService.Position;
                if (_raycastPresenter.DefaultRaycast(_cameraPresenter.Camera.ScreenPointToRay(mousePos), out var hit,
                        1000f)) _currentDrag.SetPosition(new Vector3(hit.point.x, 0f, hit.point.z));

                if (_freshlyPressed)
                    _freshlyPressed = false;
                else if (_mouseInputService.LeftClicked)
                {
                    _currentDrag.Stamp();
                    _currentDrag = null;
                }
            }
        }
    }
}