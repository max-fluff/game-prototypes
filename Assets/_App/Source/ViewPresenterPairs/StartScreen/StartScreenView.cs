using System;
using Lean.Gui;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MaxFluff.Prototypes
{
    public sealed class StartScreenView : ViewBase
    {
        [Header("Other")]
        public LeanButton LogOut;
        public LeanButton Language;
        public TextMeshProUGUI LanguageCode;
    }

    public sealed class StartScreenPresenter : PresenterBase<StartScreenView>
    {
        public event Action OnQuitClicked;
        public event Action OnLanguageClicked;
        
        public StartScreenPresenter(StartScreenView view) : base(view)
        {
            _view.LogOut.OnClick.AddListener(QuitClick);
            _view.Language.OnClick.AddListener(LanguageClick);
        }

        private void QuitClick() => OnQuitClicked?.Invoke();
        private void LanguageClick() => OnLanguageClicked?.Invoke();
        
        public void SetLanguageCode(string code) => _view.LanguageCode.text = code;
    }
}