using TMPro;

namespace MaxFluff.Prototypes
{
    public class MoneyCounterView : ViewBase
    {
        public TextMeshProUGUI label;
    }

    public class MoneyCounterPresenter : PresenterBase<MoneyCounterView>
    {
        public MoneyCounterPresenter(MoneyCounterView view) : base(view)
        {
        }

        public void SetMoneyCounter(int money) =>
            _view.label.SetText(money.ToString());
    }
}