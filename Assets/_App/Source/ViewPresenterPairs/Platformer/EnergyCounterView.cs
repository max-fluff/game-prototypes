using UnityEngine.UI;

namespace MaxFluff.Prototypes
{
    public class EnergyCounterView : ViewBase
    {
        public Slider Slider;
    }

    public class EnergyCounterPresenter : PresenterBase<EnergyCounterView>
    {
        public EnergyCounterPresenter(EnergyCounterView view) : base(view)
        {
        }

        public void UpdateEnergyValue(float energy)
        {
            _view.Slider.SetValueWithoutNotify(energy);
        }
    }
}