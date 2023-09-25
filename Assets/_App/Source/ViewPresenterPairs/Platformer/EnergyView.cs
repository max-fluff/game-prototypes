using System;

namespace MaxFluff.Prototypes
{
    public class EnergyView : ViewBase
    {
        public PlayerTrigger PlayerTrigger;
        public int EnergyValue;
    }

    public class EnergyPresenter : PresenterBase<EnergyView>
    {
        public event Action<int, EnergyPresenter> OnFillRequested;

        public EnergyPresenter(EnergyView view) : base(view) =>
            view.PlayerTrigger.OnPlayerEnter.AddListener(FillPlayerEnergy);

        private void FillPlayerEnergy() => 
            OnFillRequested?.Invoke(_view.EnergyValue, this);
    }
}