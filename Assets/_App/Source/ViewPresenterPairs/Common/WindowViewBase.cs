using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Lean.Gui;
using UnityEngine;

namespace MaxFluff.Prototypes
{
    public abstract class WindowViewBase : TransformView
    {
        public LeanWindow Window;
        public List<LeanButton> CloseButtons;
    }

    public interface IWindowPresenter
    {
        public event Action OnOpened;
        public event Action OnClosed;

        public bool IsOpened { get; }
        public Func<bool> IsOpeningAllowedDelegate { set; }

        public bool MayBeClosed { get; }
        public bool NeedBlocker { get; }
        public bool AllowAnotherWindowOnTop { get; }
        public bool ClosesOnOutsideClick { get; }
        public bool IgnoreOutsideClickClosing { get; set; }

        public Transform Transform { get; }
        public Type ViewType { get; }
        
        public void Open();
        public void Close();
    }

    public abstract class WindowPresenterBase<TView> : TransformPresenter<TView>, IWindowPresenter
        where TView : WindowViewBase
    {
        public event Action OnOpened;
        public event Action OnClosed;

        protected WindowPresenterBase(TView view) : base(view)
        {
            view.Window.OnOn.AddListener(SendOnOpen);
            view.Window.OnOff.AddListener(SendOnClose);

            if (MayBeClosed)
                foreach (var viewCloseButton in view.CloseButtons)
                    viewCloseButton.OnClick.AddListener(view.Window.TurnOff);
        }

        public Func<bool> IsOpeningAllowedDelegate { private get; set; }

        public bool IsOpened => _view.Window.On;

        public virtual bool MayBeClosed => true;
        public virtual bool NeedBlocker => true;
        public virtual bool AllowAnotherWindowOnTop => true;
        public virtual bool ClosesOnOutsideClick => false;
        public bool IgnoreOutsideClickClosing { get; set; }

        public Type ViewType => typeof(TView);

        private void SendOnOpen() => OnOpened?.Invoke();
        private void SendOnClose() => OnClosed?.Invoke();

        protected UniTask WaitWhileOpened() =>
            UniTask.WaitWhile(Opened);

        private bool Opened() => IsOpened;

        protected bool IsOpeningAllowed =>
            IsOpeningAllowedDelegate?.Invoke() ?? false;

        public virtual void Open()
        {
            if (!IsOpeningAllowed)
                throw new WindowOpeningNotAllowedException(this);
            
            _view.Window.TurnOn();
        }

        public virtual void Close() =>
            _view.Window.TurnOff();
    }
}