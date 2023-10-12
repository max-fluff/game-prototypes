using System;
using TMPro;

namespace MaxFluff.Prototypes
{
    public class TimerVisualizerView : ViewBase
    {
        public TextMeshProUGUI Label;
    }

    public class TimerVisualizerPresenter : PresenterBase<TimerVisualizerView>
    {
        public TimerVisualizerPresenter(TimerVisualizerView view) : base(view)
        {
        }

        public void SetTime(float seconds)
        {
            var time = TimeSpan.FromSeconds(seconds);
            _view.Label.SetText($"{time.Minutes}:{time.Seconds}:{time.Milliseconds}");
        }
    }
}