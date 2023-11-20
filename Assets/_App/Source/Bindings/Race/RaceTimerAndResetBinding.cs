using UnityEngine;

namespace MaxFluff.Prototypes
{
    public class RaceTimerAndResetBinding : IInitBinding, IRunBinding
    {
        private readonly TimeResultWindowPresenter _timeResultWindow;
        private readonly FailWindowPresenter _failWindow;
        private readonly TimerVisualizerPresenter _timerVisualizerPresenter;
        private readonly DistanceVisualizerPresenter _distanceVisualizerPresenter;
        private readonly BordersPresenter _bordersPresenter;
        private readonly FinishPresenter _finishPresenter;
        private readonly KeyboardInputService _keyboardInputService;
        private readonly GravityService _gravityService;
        private readonly LevelGeneratorPresenter _levelGeneratorPresenter;
        private readonly RacePlayerPresenter _player;

        private float _timerInSeconds;
        private int _distanceCached;

        public RaceTimerAndResetBinding(
            TimeResultWindowPresenter timeResultWindow,
            FailWindowPresenter failWindow,
            TimerVisualizerPresenter timerVisualizerPresenter,
            DistanceVisualizerPresenter distanceVisualizerPresenter,
            BordersPresenter bordersPresenter,
            FinishPresenter finishPresenter,
            KeyboardInputService keyboardInputService,
            GravityService gravityService,
            LevelGeneratorPresenter levelGeneratorPresenter,
            RacePlayerPresenter player)
        {
            _timeResultWindow = timeResultWindow;
            _failWindow = failWindow;
            _timerVisualizerPresenter = timerVisualizerPresenter;
            _distanceVisualizerPresenter = distanceVisualizerPresenter;
            _bordersPresenter = bordersPresenter;
            _finishPresenter = finishPresenter;
            _keyboardInputService = keyboardInputService;
            _gravityService = gravityService;
            _levelGeneratorPresenter = levelGeneratorPresenter;
            _player = player;
        }

        public void Init()
        {
            _timeResultWindow.OnClosed += ResetGame;
            _failWindow.OnClosed += ResetGame;
            _keyboardInputService.OnInputAction += ProcessInput;
            _bordersPresenter.OnTriggerEntered += ShowWinWindow;

            _levelGeneratorPresenter.OnRequestedPlayerMove += MovePlayer;
        }

        private void MovePlayer(Vector3 delta)
        {
            _distanceCached = xDistance;
            _player.Position += delta;
        }

        private void ShowWinWindow(Collider other)
        {
            if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
            {
                ShowWinWindow();
            }
        }

        private void ShowWinWindow()
        {
            _timeResultWindow.SetTime(_timerInSeconds);
            _timeResultWindow.SetDistance(xDistance);
            _timeResultWindow.Open();
            ResetPlayer();
        }

        private void ProcessInput(Actions action)
        {
            if (action == Actions.R)
                ShowWinWindow();
        }

        private void ResetGame()
        {
            _timerInSeconds = 0;
            _levelGeneratorPresenter.GenerateNewLevel();
        }

        private void ResetPlayer()
        {
            _distanceCached = 0;
            _player.ResetPlayer();
            _gravityService.SetGravityDirection(Vector3.down);
        }

        public void Run()
        {
            if (!_timeResultWindow.IsOpened && !_failWindow.IsOpened)
            {
                _timerInSeconds += Time.deltaTime;
                _timerVisualizerPresenter.SetTime(_timerInSeconds);
                _distanceVisualizerPresenter.SetDistance(xDistance);
            }
            else
            {
                if (_keyboardInputService.IsKeyDown(KeyCode.Return) || _keyboardInputService.IsKeyDown(KeyCode.Space))
                    _timeResultWindow.Close();
            }
        }

        private int xDistance => (int)Mathf.Abs(_player.Position.x - _player.InitPosition.x) + _distanceCached;
    }
}