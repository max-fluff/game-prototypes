using UnityEngine.UI;

namespace MaxFluff.Prototypes
{
    public class PopularityCounterView : ViewBase
    {
        public Slider Slider;
    }

    public class PopularityCounterPresenter : PresenterBase<PopularityCounterView>
    {
        public PopularityCounterPresenter(PopularityCounterView view) : base(view)
        {
        }

        public void UpdateValue(float energy)
        {
            _view.Slider.SetValueWithoutNotify(energy);
        }
    }
}