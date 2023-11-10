using Lean.Transition;
using UnityEngine.UI;

namespace MaxFluff.Prototypes.FPS
{
    public class HealthVisualizationView : ViewBase
    {
        public Slider Slider;
        public LeanPlayer DamageOverlayTransition;
    }

    public class HealthVisualizationPresenter : PresenterBase<HealthVisualizationView>
    {
        public HealthVisualizationPresenter(HealthVisualizationView view) : base(view)
        {
        }

        public void UpdateValue(float value)
        {
            _view.Slider.value = value;
            _view.DamageOverlayTransition.Begin();
        }
    }
}