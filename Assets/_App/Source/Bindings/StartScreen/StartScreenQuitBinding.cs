using UnityEngine;

namespace MaxFluff.Prototypes
{
    public class StartScreenQuitBinding : IInitBinding, IDestroyBinding
    {
        private readonly StartScreenPresenter _startScreenPresenter;
        private readonly QuitWindowPresenter _quitWindow;
        private readonly KeyboardInputService _keyboardInputService;

        public StartScreenQuitBinding(
            StartScreenPresenter startScreenPresenter,
            QuitWindowPresenter quitWindow,
            KeyboardInputService keyboardInputService
        )
        {
            _startScreenPresenter = startScreenPresenter;
            _quitWindow = quitWindow;
            _keyboardInputService = keyboardInputService;
        }

        public void Init()
        {
            _keyboardInputService.OnInputAction += ProcessInputAction;
            _startScreenPresenter.OnQuitClicked += QuitClicked;
            _quitWindow.OnQuitRequested += Quit;
        }

        private void QuitClicked()
        {
            _quitWindow.Open();
        }

        private void ProcessInputAction(Actions action)
        {
            if (action == Actions.Esc)
            {
                if (_quitWindow.IsOpened)
                    _quitWindow.Close();
                else
                    _quitWindow.Open();
            }
        }

        private void Quit() => Application.Quit();

        public void Destroy() => 
            _keyboardInputService.OnInputAction -= ProcessInputAction;
    }
}