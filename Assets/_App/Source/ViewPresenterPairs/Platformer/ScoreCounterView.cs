using TMPro;

namespace MaxFluff.Prototypes
{
    public class ScoreCounterView : ViewBase
    {
        public TextMeshProUGUI label;
    }

    public class ScoreCounterPresenter : PresenterBase<ScoreCounterView>
    {
        public ScoreCounterPresenter(ScoreCounterView view) : base(view)
        {
        }

        public void SetScore(int score)
        {
            _view.label.SetText($"SCORE: {score}");
        }
    }
}