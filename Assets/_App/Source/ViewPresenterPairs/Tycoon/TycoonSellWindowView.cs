using Lean.Gui;
using TMPro;

namespace MaxFluff.Prototypes
{
    public class TycoonSellWindowView : WindowViewBase
    {
        public TextMeshProUGUI price;
        public LeanButton sellButton;
    }

    public class TycoonSellWindowPresenter : WindowPresenterBase<TycoonSellWindowView>
    {
        public bool IsSelling = false;

        public override bool NeedBlocker => true;

        public TycoonSellWindowPresenter(TycoonSellWindowView view) : base(view)
        {
            view.sellButton.OnClick.AddListener(Sell);
        }

        public void SetPrice(int price)
        {
            _view.price.SetText(price + "$");
        }

        private void Sell()
        {
            IsSelling = true;

            Close();

            IsSelling = false;
        }
    }
}