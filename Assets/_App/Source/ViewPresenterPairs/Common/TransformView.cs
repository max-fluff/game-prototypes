using UnityEngine;

namespace MaxFluff.Prototypes
{
    public class TransformView : ViewBase
    {
    }

    public interface ITransformPresenter
    {
        public Transform Transform { get; }
    }

    public class TransformPresenter<TView> : PresenterBase<TView>, ITransformPresenter where TView : TransformView
    {
        protected TransformPresenter(TView view) : base(view)
        {
        }

        public Transform Transform => _view.transform;
    }
}