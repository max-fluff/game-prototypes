using TMPro;

namespace MaxFluff.Prototypes
{
    public class FpsCounterView : ViewBase
    {
        public TextMeshProUGUI FPSCounter;
    }

    public sealed class FpsCounterPresenter : PresenterBase<FpsCounterView>
    {
        public FpsCounterPresenter(FpsCounterView view) : base(view)
        {
        }

        public void UpdateFPS(int fps)
        {
            _view.FPSCounter.SetText(fps.ToString());
        }
    }
}