using UnityEngine;

namespace MaxFluff.Prototypes
{
    public class CameraView : TransformView
    {
        public Camera Camera;
    }

    public interface ICameraPresenter
    {
        public Camera Camera { get; }
    }

    public class CameraPresenter<TView> : TransformPresenter<TView>, ICameraPresenter where TView : CameraView
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