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
        public GameObject Signature;
        public GameObject Stamp;
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
            await UniTask.Delay((int) (_view.PuttingAwayLength * 1000));
            Destroy();
        }

        public void SetStamp(Vector3 worldPos)
        {
            var stampParent = _view.Stamp.transform.parent;
            var localPos = stampParent.InverseTransformPoint(worldPos);
            localPos.z = 0f;
            var newStamp = Object.Instantiate(_view.Stamp, stampParent);
            newStamp.transform.localPosition = localPos;
            newStamp.SetActive(true);
            _gotStamp = true;
        }

        public void SetSignature(Vector3 worldPos)
        {
            var signatureParent = _view.Signature.transform.parent;
            var localPos = signatureParent.InverseTransformPoint(worldPos);
            localPos.z = 0f;
            var newSignature = Object.Instantiate(_view.Signature, signatureParent);
            newSignature.transform.localPosition = localPos;
            newSignature.SetActive(true);
            _gotSignature = true;
        }
    }
}