using System;
using Cysharp.Threading.Tasks;
using Lean.Transition;
using UnityEngine;

namespace MaxFluff.Prototypes
{
    public abstract class DraggableObjectView : ViewBase
    {
        public TapProcessor TapProcessor;
        public LeanPlayer PuttingBackTransition;
        public LeanPlayer GettingUpTransition;
        public LeanPlayer StampTransition;
        public AudioSource StampAudio;
        public float StampTime;
    }

    public abstract class DraggableObjectPresenter<T> : PresenterBase<T>, IDraggableObjectPresenter
        where T : DraggableObjectView
    {
        public event Action<IDraggableObjectPresenter> OnStartDrag;

        protected DraggableObjectPresenter(T view) : base(view)
        {
            view.TapProcessor.OnTappped.AddListener(OnTapped);
        }

        private void OnTapped()
        {
            OnStartDrag?.Invoke(this);
            StartDrag();
            _view.TapProcessor.enabled = false;
        }

        private void StartDrag() =>
            _view.GettingUpTransition.Begin();

        public void SetPosition(Vector3 position)
        {
            var transformPosition = _view.transform.position;
            transformPosition.x = position.x;
            transformPosition.z = position.z;
            _view.transform.position = transformPosition;
        }

        public void Reset()
        {
            _view.PuttingBackTransition.Begin();
            _view.TapProcessor.enabled = true;
        }

        public void Stamp()
        {
            StampAsync().Forget();
        }

        private async UniTask StampAsync()
        {
            _view.StampTransition.Begin();
            _view.StampAudio.PlayOneShot(_view.StampAudio.clip);

            await UniTask.Delay((int)(_view.StampTime * 1000));

            Reset();
        }
    }

    public interface IDraggableObjectPresenter
    {
        public void SetPosition(Vector3 position);
        public void Stamp();
        public void Reset();
    }
}