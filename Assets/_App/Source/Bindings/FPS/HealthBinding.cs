using System;
using MaxFluff.Prototypes.FPS;

namespace MaxFluff.Prototypes
{
    public class HealthBinding : IInitBinding, IStateChangerBinding
    {
        private readonly FPSPlayerPresenter _player;
        private readonly HealthVisualizationPresenter _healthVisualizationPresenter;
        private readonly FailWindowPresenter _failWindow;
        private readonly CursorService _cursorService;

        public HealthBinding(
            FPSPlayerPresenter player,
            HealthVisualizationPresenter healthVisualizationPresenter,
            FailWindowPresenter failWindow,
            CursorService cursorService)
        {
            _player = player;
            _healthVisualizationPresenter = healthVisualizationPresenter;
            _failWindow = failWindow;
            _cursorService = cursorService;
        }

        public void Init()
        {
            _player.OnHealthUpdate += UpdateHealth;
            _failWindow.OnClosed += Restart;
            _healthVisualizationPresenter.UpdateValue(_player.Health);
        }

        private void Restart()
        {
            OnStateChangeRequested?.Invoke(new StartScreenState());
        }

        private void UpdateHealth(float health)
        {
            _healthVisualizationPresenter.UpdateValue(health);

            if (health < 0.01)
            {
                _cursorService.IsCursorVisible = true;
                _failWindow.Open();
            }
        }

        public event Action<IAppState> OnStateChangeRequested;
    }
}