using UnityEngine;

namespace MaxFluff.Prototypes
{
    public class NarrativeBinding : IInitBinding, IRunBinding
    {
        private readonly PenPresenter _penPresenter;
        private readonly StampPresenter _stampPresenter;
        private readonly RaycastPresenter _raycastPresenter;
        private readonly CameraPresenter _cameraPresenter;
        private readonly SheetStackPresenter _sheetStackPresenter;
        private readonly MouseInputService _mouseInputService;

        private IDraggableObjectPresenter _currentDrag;
        private bool _freshlyPressed;

        public NarrativeBinding(
            PenPresenter penPresenter,
            StampPresenter stampPresenter,
            RaycastPresenter raycastPresenter,
            CameraPresenter cameraPresenter,
            SheetStackPresenter sheetStackPresenter,
            MouseInputService mouseInputService)
        {
            _penPresenter = penPresenter;
            _stampPresenter = stampPresenter;
            _raycastPresenter = raycastPresenter;
            _cameraPresenter = cameraPresenter;
            _sheetStackPresenter = sheetStackPresenter;
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
                var wasHit = _raycastPresenter.DefaultRaycast(_cameraPresenter.Camera.ScreenPointToRay(mousePos),
                    out var hit,
                    1000f);
                if (wasHit) _currentDrag.SetPosition(new Vector3(hit.point.x, 0f, hit.point.z));

                if (_freshlyPressed)
                    _freshlyPressed = false;
                else if (_mouseInputService.Up)
                {
                    if (wasHit && hit.collider.GetComponentInParent<SingleSheetView>())
                    {
                        _currentDrag.Stamp();
                        switch (_currentDrag)
                        {
                            case StampPresenter _:
                                _sheetStackPresenter.SetStampOnCurrent();
                                break;
                            case PenPresenter _:
                                _sheetStackPresenter.SetSignatureOnCurrent();
                                break;
                        }

                        _currentDrag = null;
                    }
                    else
                    {
                        _currentDrag.Reset();
                        _currentDrag = null;
                    }
                }
            }
        }
    }
}