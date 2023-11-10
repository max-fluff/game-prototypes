using Lean.Gui;

namespace MaxFluff.Prototypes
{
    public class TycoonBarWindowView : WindowViewBase
    {
        public LeanButton buyButton;
    }

    public class TycoonBarWindowPresenter : WindowPresenterBase<TycoonBarWindowView>
    {
        public bool IsBuying;

        public override bool NeedBlocker => true;

        public TycoonBarWindowPresenter(TycoonBarWindowView view) : base(view)
        {
            view.buyButton.OnClick.AddListener(Buy);
        }

        private void Buy()
        {
            IsBuying = true;

            Close();

            IsBuying = false;
        }
    }
}