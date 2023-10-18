using TMPro;

namespace MaxFluff.Prototypes
{
    public class CounterView : ViewBase
    {
        public TextMeshProUGUI Text;
    }

    public class CounterPresenter : PresenterBase<CounterView>
    {
        public CounterPresenter(CounterView view) : base(view)
        {
        }

        public void SetText(int text) => _view.Text.SetText(text.ToString());
    }
}