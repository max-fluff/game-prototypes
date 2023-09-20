using UnityEngine;

namespace MaxFluff.Prototypes
{
    public class CameraView : ViewBase
    {
        public Camera Camera;
    }

    public interface ICameraPresenter
    {
        public Camera Camera { get; }
    }

    public class CameraPresenter<TView> : PresenterBase<TView>, ICameraPresenter where TView : CameraView
    {
        public CameraPresenter(TView view) : base(view)
        {
        }
        public Camera Camera => _view.Camera;
    }
    
    public class CameraPresenter : CameraPresenter<CameraView>
    {
        public CameraPresenter(CameraView view) : base(view)
        {
        }
    }
}