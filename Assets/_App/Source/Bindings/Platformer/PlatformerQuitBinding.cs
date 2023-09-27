using System;

namespace MaxFluff.Prototypes
{
    public class PlatformerQuitBinding : IInitBinding, IStateChangerBinding, IDestroyBinding
    {
        private readonly QuitWindowPresenter _quitWindow;
        private readonly KeyboardInputService _keyboardInputService;
        private readonly LoadingWindowPresenter _loadingWindowPresenter;

        public event Action<IAppState> OnStateChangeRequested;


        public PlatformerQuitBinding(
            QuitWindowPresenter quitWindow,
            KeyboardInputService keyboardInputService,
            LoadingWindowPresenter loadingWindowPresenter)
        {
            _quitWindow = quitWindow;
            _keyboardInputService = keyboardInputService;
            _loadingWindowPresenter = loadingWindowPresenter;
        }

        public void Init()
        {
            _keyboardInputService.OnInputAction += ProcessInputAction;
            _quitWindow.OnQuitRequested += QuitToStartScreen;
        }

        private void QuitToStartScreen()
        {
            _loadingWindowPresenter.Open();
            OnStateChangeRequested?.Invoke(new StartScreenState());
        }

        private void ProcessInputAction(Actions action)
        {
            if (action != Actions.Esc) return;

            if (_quitWindow.IsOpened)
                _quitWindow.Close();
            else
                _quitWindow.Open();
        }

        public void Destroy()
        {
            _keyboardInputService.OnInputAction -= ProcessInputAction;
        }
    }
}