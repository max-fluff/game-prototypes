using UnityEngine;

namespace MaxFluff.Prototypes
{
    public class RaceTimerAndResetBinding : IInitBinding, IRunBinding
    {
        private readonly TimeResultWindowPresenter _timeResultWindow;
        private readonly FailWindowPresenter _failWindow;
        private readonly TimerVisualizerPresenter _timerVisualizerPresenter;
        private readonly BordersPresenter _bordersPresenter;
        private readonly FinishPresenter _finishPresenter;
        private readonly KeyboardInputService _keyboardInputService;
        private readonly GravityService _gravityService;
        private readonly RacePlayerPresenter _player;

        private float _timerInSeconds;

        public RaceTimerAndResetBinding(
            TimeResultWindowPresenter timeResultWindow,
            FailWindowPresenter failWindow,
            TimerVisualizerPresenter timerVisualizerPresenter,
            BordersPresenter bordersPresenter,
            FinishPresenter finishPresenter,
            KeyboardInputService keyboardInputService,
            GravityService gravityService,
            RacePlayerPresenter player)
        {
            _timeResultWindow = timeResultWindow;
            _failWindow = failWindow;
            _timerVisualizerPresenter = timerVisualizerPresenter;
            _bordersPresenter = bordersPresenter;
            _finishPresenter = finishPresenter;
            _keyboardInputService = keyboardInputService;
            _gravityService = gravityService;
            _player = player;
        }

        public void Init()
        {
            _timeResultWindow.OnClosed += ResetTimer;
            _failWindow.OnClosed += ResetTimer;
            _keyboardInputService.OnInputAction += ProcessInput;
            _bordersPresenter.OnTriggerEntered += ShowFailWindow;
            _finishPresenter.OnTriggerEntered += ShowWinWindow;
        }

        private void ShowWinWindow(Collider other)
        {
            if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
            {
                _timeResultWindow.SetTime(_timerInSeconds);
                _timeResultWindow.Open();
                ResetPlayer();
            }
        }

        private void ShowFailWindow(Collider _)
        {
            _failWindow.Open();
            ResetPlayer();
        }

        private void ProcessInput(Actions action)
        {
            if (action == Actions.R)
            {
                ResetPlayer();
                ResetTimer();
            }
        }

        private void ResetTimer()
        {
            _timerInSeconds = 0;
        }

        private void ResetPlayer()
        {
            _player.ResetPlayer();
            _gravityService.SetGravityDirection(Vector3.down);
        }

        public void Run()
        {
            if (!_timeResultWindow.IsOpened && !_failWindow.IsOpened)
            {
                _timerInSeconds += Time.deltaTime;
                _timerVisualizerPresenter.SetTime(_timerInSeconds);
            }
        }
    }
}