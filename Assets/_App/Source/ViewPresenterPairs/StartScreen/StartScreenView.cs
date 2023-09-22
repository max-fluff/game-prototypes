using System;
using Lean.Gui;

using UnityEngine;
namespace MaxFluff.Prototypes
{
    public sealed class StartScreenView : ViewBase
    {
        [Header("Other")]
        public LeanButton Quit;
    }

    public sealed class StartScreenPresenter : PresenterBase<StartScreenView>
    {
        public event Action OnQuitClicked;
        
        public StartScreenPresenter(StartScreenView view) : base(view)
        {
            _view.Quit.OnClick.AddListener(QuitClick);
        }

        private void QuitClick() => OnQuitClicked?.Invoke();
    }
}