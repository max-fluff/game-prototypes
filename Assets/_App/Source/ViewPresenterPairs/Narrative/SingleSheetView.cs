using Cysharp.Threading.Tasks;
using Lean.Transition;
using UnityEngine;

namespace MaxFluff.Prototypes
{
    public class SingleSheetView : ViewBase
    {
        public TapProcessor TapProcessor;
        public LeanPlayer GetTransition;
        public LeanPlayer PutAwayTransition;
        public AudioSource Audio;
        public float PuttingAwayLength;
    }

    public class SingleSheetPresenter : PresenterBase<SingleSheetView>
    {
        private bool _gotSignature;
        private bool _gotStamp;
        private bool _isDestroyed;

        public SingleSheetPresenter(SingleSheetView view) : base(view)
        {
            _view.TapProcessor.OnDraggedRight.AddListener(PlayPutAwayTransition);
        }

        public void PlayGetTransition()
        {
            _view.GetTransition.Begin();
            _view.Audio.PlayOneShot(_view.Audio.clip);
        }

        private void PlayPutAwayTransition()
        {
            if (_gotSignature && _gotStamp && !_isDestroyed)
            {
                _view.PutAwayTransition.Begin();
                _view.Audio.PlayOneShot(_view.Audio.clip);
                DelayedDestroy().Forget();
                _view.TapProcessor.enabled = false;
            }
        }

        private async UniTask DelayedDestroy()
        {
            _isDestroyed = true;
            await UniTask.Delay((int)(_view.PuttingAwayLength * 1000));
            Destroy();
        }

        public void SetStamp() => _gotStamp = true;

        public void SetSignature() => _gotSignature = true;
    }
}