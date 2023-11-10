using System;
using UnityEngine;

namespace MaxFluff.Prototypes
{
    public class GameQuitBinding : IInitBinding, IStateChangerBinding, IDestroyBinding
    {
        private readonly QuitWindowPresenter _quitWindow;
        private readonly KeyboardInputService _keyboardInputService;
        private readonly LoadingWindowPresenter _loadingWindowPresenter;
        private readonly CursorService _cursorService;

        public event Action<IAppState> OnStateChangeRequested;

        private bool _cursorState;


        public GameQuitBinding(
            QuitWindowPresenter quitWindow,
            KeyboardInputService keyboardInputService,
            LoadingWindowPresenter loadingWindowPresenter,
            CursorService cursorService)
        {
            _quitWindow = quitWindow;
            _keyboardInputService = keyboardInputService;
            _loadingWindowPresenter = loadingWindowPresenter;
            _cursorService = cursorService;
        }

        public void Init()
        {
            _keyboardInputService.OnInputAction += ProcessInputAction;
            _quitWindow.OnQuitRequested += QuitToStartScreen;

            _quitWindow.OnOpened += ShowCursor;
            _quitWindow.OnClosed += ResetCursor;
        }

        private void ShowCursor()
        {
            _cursorState = _cursorService.IsCursorVisible;
            _cursorService.IsCursorVisible = true;
        }

        private void ResetCursor()
        {
            _cursorService.IsCursorVisible = _cursorState;
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