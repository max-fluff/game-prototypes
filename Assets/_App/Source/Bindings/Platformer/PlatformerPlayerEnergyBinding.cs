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
            var sum = 0;
            foreach (var energy in _energyContainerPresenter.Transform.GetComponentsInChildren<EnergyView>())
            {
                sum += energy.EnergyValue;
                var energyPresenter = new EnergyPresenter(energy);
                energyPresenter.OnFillRequested += FillPlayerEnergy;
            }

            _scoreCounterPresenter.SetScore(_score);

            _player.OnEnergyAmountUpdate += _energyCounterPresenter.UpdateEnergyValue;

            _player.Energy = PlatformerPlayerPresenter.MaxEnergy;
        }

        private void FillPlayerEnergy(int amount, EnergyPresenter presenter)
        {
            var wasFull = _player.Energy == PlatformerPlayerPresenter.MaxEnergy;
            
            _player.PlayCoinSound();
            _player.Energy += amount;

            if (!wasFull && _player.Energy == PlatformerPlayerPresenter.MaxEnergy)
                _player.PlayFullSound();
            
            _score += amount;
            _scoreCounterPresenter.SetScore(_score);
            presenter.OnFillRequested -= FillPlayerEnergy;
            presenter.Destroy();
        }
    }
}