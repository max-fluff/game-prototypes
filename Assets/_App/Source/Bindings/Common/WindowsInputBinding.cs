namespace Omega.Kulibin
{
    public sealed class WindowsInputBinding : IRunBinding
    {
        private readonly WindowsOrganizerPresenter _windowsOrganizer;
        private readonly MouseInputService _mouse;
        private readonly KeyboardInputService _keyboard;
        private readonly RaycastPresenter _raycast;

        public WindowsInputBinding(
            WindowsOrganizerPresenter windowsOrganizer,
            MouseInputService mouse,
            KeyboardInputService keyboard,
            RaycastPresenter raycast
            )
        {
            _windowsOrganizer = windowsOrganizer;
            _mouse = mouse;
            _keyboard = keyboard;
            _raycast = raycast;
        }

        public void Run()
        {
            _windowsOrganizer.CacheOpenedWindowsCount();
            
            if (_windowsOrganizer.KeyboardInputRequired &&
                _keyboard.IsKeyDown(_windowsOrganizer.CloseKey))
            {
                _windowsOrganizer.ProcessCloseKey();
            }

            if (_windowsOrganizer.MouseInputRequired &&
                _mouse.Down)
            {
                var mousePosition = _mouse.Hover.ScreenPosition;
                _raycast.GraphicRaycast(mousePosition, out var results);
                _windowsOrganizer.ProcessMouseDown(results);
            }
        }
    }
}