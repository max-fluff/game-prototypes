using System;
using TMPro;

namespace MaxFluff.Prototypes
{
    public class DistanceVisualizerView : ViewBase
    {
        public TextMeshProUGUI Label;
    }

    public class DistanceVisualizerPresenter : PresenterBase<DistanceVisualizerView>
    {
        public DistanceVisualizerPresenter(DistanceVisualizerView view) : base(view)
        {
        }

        public void SetDistance(int distance)
        {
            _view.Label.SetText($"{distance}");
        }
    }
}