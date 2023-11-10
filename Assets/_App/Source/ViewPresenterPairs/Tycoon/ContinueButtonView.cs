using System;
using Lean.Gui;

namespace MaxFluff.Prototypes
{
    public class ContinueButtonView : ViewBase
    {
        public LeanButton Button;
    }

    public class ContinueButtonPresenter : PresenterBase<ContinueButtonView>
    {
        public event Action OnContinue;

        public ContinueButtonPresenter(ContinueButtonView view) : base(view)
        {
            view.Button.OnClick.AddListener(SendOnContinue);
        }

        public void SendOnContinue() => OnContinue?.Invoke();

        public void SetActive(bool isActive)
        {
            _view.gameObject.SetActive(isActive);
        }
    }
}