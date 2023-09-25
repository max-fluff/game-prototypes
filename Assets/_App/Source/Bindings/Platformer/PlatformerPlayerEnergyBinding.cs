namespace MaxFluff.Prototypes
{
    public class PlatformerPlayerEnergyBinding : IInitBinding
    {
        private readonly EnergyContainerPresenter _energyContainerPresenter;
        private readonly ScoreCounterPresenter _scoreCounterPresenter;
        private readonly EnergyCounterPresenter _energyCounterPresenter;
        private readonly PlatformerPlayerPresenter _player;

        private int _score;

        public PlatformerPlayerEnergyBinding(
            EnergyContainerPresenter energyContainerPresenter,
            ScoreCounterPresenter scoreCounterPresenter,
            EnergyCounterPresenter energyCounterPresenter,
            PlatformerPlayerPresenter player)
        {
            _energyContainerPresenter = energyContainerPresenter;
            _scoreCounterPresenter = scoreCounterPresenter;
            _energyCounterPresenter = energyCounterPresenter;
            _player = player;
        }

        public void Init()
        {
            foreach (var energy in _energyContainerPresenter.Transform.GetComponentsInChildren<EnergyView>())
            {
                var energyPresenter = new EnergyPresenter(energy);
                energyPresenter.OnFillRequested += FillPlayerEnergy;
            }

            _scoreCounterPresenter.SetScore(_score);

            _player.OnEnergyAmountUpdate += _energyCounterPresenter.UpdateEnergyValue;

            _player.Energy = PlatformerPlayerPresenter.MaxEnergy;
        }

        private void FillPlayerEnergy(int amount, EnergyPresenter presenter)
        {
            _player.Energy += amount;
            _score += amount;
            _scoreCounterPresenter.SetScore(_score);
            presenter.OnFillRequested -= FillPlayerEnergy;
            presenter.Destroy();
        }
    }
}