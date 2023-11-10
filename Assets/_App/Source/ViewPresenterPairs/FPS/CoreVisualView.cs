using TMPro;

namespace MaxFluff.Prototypes.FPS
{
    public class CoreVisualView : ViewBase
    {
        public TextMeshProUGUI Counter;
    }

    public class CoreVisualPresenter : PresenterBase<CoreVisualView>
    {
        public CoreVisualPresenter(CoreVisualView view) : base(view)
        {
        }

        public void SetCount(int amount, int outOf) => 
            _view.Counter.SetText($"{amount}/{outOf}");
    }
}