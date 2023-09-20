using System;

namespace MaxFluff.Prototypes
{
    public sealed class ButtonWindowPair : IDisposable
    {
        private readonly IButtonPresenter _button;
        private readonly IWindowPresenter _window;

        public ButtonWindowPair(
            IButtonPresenter button,
            IWindowPresenter window)
        {
            _button = button;
            _window = window;
            
            button.OnClick += ToggleWindow;
            button.OnDown += CheckWindowOpened;
        }

        private void CheckWindowOpened()
        {
            if (_window.IsOpened &&
                _window.ClosesOnOutsideClick)
            {
                _window.IgnoreOutsideClickClosing = true;
            }
        }

        private void ToggleWindow()
        {
            if (_window.IsOpened)
            {
                _window.Close();
            }
            else
            {
                _window.Open();
                if (_window.ClosesOnOutsideClick)
                    _window.IgnoreOutsideClickClosing = false;
            }
        }

        public void Dispose()
        {
            _button.OnClick -= ToggleWindow;
            _button.OnDown -= CheckWindowOpened;
        }
    }
}