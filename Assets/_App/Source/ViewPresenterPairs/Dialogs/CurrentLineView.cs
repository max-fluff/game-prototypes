using TMPro;

namespace MaxFluff.Prototypes
{
    public class CurrentLineView : ViewBase
    {
        public TextMeshProUGUI Text;
    }

    public class CurrentLinePresenter : PresenterBase<CurrentLineView>
    {
        public CurrentLinePresenter(CurrentLineView view) : base(view)
        {
            SetText(string.Empty);
        }

        public void SetText(string text) => _view.Text.SetText(text);
    }
}