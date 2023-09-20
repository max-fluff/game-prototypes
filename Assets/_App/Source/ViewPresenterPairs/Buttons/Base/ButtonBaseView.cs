using System;
using Lean.Gui;
using UnityEngine;

namespace MaxFluff.Prototypes
{
    public abstract class ButtonBaseView : ViewBase
    {
        public LeanButton Button;
    }

    public interface IButtonPresenter
    {
        public event Action OnClick;
        public event Action OnDown;
    }

    public abstract class ButtonBasePresenter<T> : PresenterBase<T>, IButtonPresenter where T : ButtonBaseView
    {
        public event Action OnClick;
        public event Action OnDown;

        protected ButtonBasePresenter(T view) : base(view)
        {
            _view.Button.OnClick.AddListener(SendOnClick);
            _view.Button.OnDown.AddListener(SendOnDown);
        }

        public Vector3 ButtonPosition => _view.Button.transform.position;

        public void SetButtonActive(bool value) => _view.Button.gameObject.SetActive(value);

        private void SendOnClick() => OnClick?.Invoke();
        private void SendOnDown() => OnDown?.Invoke();
    }
}