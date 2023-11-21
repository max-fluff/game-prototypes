using System;
using Lean.Transition;
using UnityEngine;

namespace MaxFluff.Prototypes.Narrative
{
    public abstract class DraggableObjectView : ViewBase
    {
        public TapProcessor TapProcessor;
        public LeanPlayer PuttingBackTransition;
        public LeanPlayer GettingUpTransition;
    }

    public abstract class DraggableObjectPresenter : PresenterBase<DraggableObjectView>
    {
        public event Action OnStartDrag;

        protected DraggableObjectPresenter(DraggableObjectView view) : base(view)
        {
            view.TapProcessor.OnTappped.AddListener(OnTapped);
        }

        private void OnTapped()
        {
            OnStartDrag?.Invoke();
            _view.TapProcessor.enabled = false;
        }

        public void StartDrag()
        {
            _view.GettingUpTransition.Begin();
        }

        public void SetPosition(Vector2 position)
        {
            var transformPosition = _view.transform.position;
            transformPosition.x = position.x;
            transformPosition.z = position.y;
            _view.transform.position = transformPosition;
        }

        public void Reset()
        {
            _view.PuttingBackTransition.Begin();
            _view.TapProcessor.enabled = true;
        }
    }
}