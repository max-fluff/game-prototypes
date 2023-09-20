using DG.Tweening;
using I2.Loc;
using UnityEngine;

namespace Omega.Kulibin
{
    public sealed class LoadingWindowView : WindowViewBase
    {
        public RectTransform ImageInner;
        public RectTransform ImageOuter;

        public float RotationDuration;

        public Localize Label;
    }

    public sealed class LoadingWindowPresenter : WindowPresenterBase<LoadingWindowView>
    {
        public LoadingWindowPresenter(LoadingWindowView view) : base(view)
        {
        }

        public override bool MayBeClosed => false;

        public override void Open()
        {
            Open(ScriptTerms.StartScreenScene.Loading);
        }

        public void Open(string labelTerm)
        {
            base.Open();
            
            _view.Label.SetTerm(labelTerm);

            _view.ImageInner.DOLocalRotate(new Vector3(0f, 0f, 360f), _view.RotationDuration, RotateMode.FastBeyond360)
                .SetLoops(-1).SetEase(Ease.Linear);
            _view.ImageOuter.DOLocalRotate(new Vector3(0f, 0f, -360f), _view.RotationDuration, RotateMode.FastBeyond360)
                .SetLoops(-1).SetEase(Ease.Linear);
        }
    }
}