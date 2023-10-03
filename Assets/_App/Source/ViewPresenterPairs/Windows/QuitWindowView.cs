using System;
using Lean.Gui;

namespace MaxFluff.Prototypes
{
    public class QuitWindowView : WindowViewBase
    {
        public LeanButton QuitButton;
    }

    public class QuitWindowPresenter : WindowPresenterBase<QuitWindowView>
    {
        public override bool MayBeClosed => true;

        public event Action OnQuitRequested; 

        public QuitWindowPresenter(QuitWindowView view) : base(view)
        {
            _view.QuitButton.OnClick.AddListener(SendOnQuitRequested);
        }

        private void SendOnQuitRequested() => OnQuitRequested?.Invoke();
    }
}