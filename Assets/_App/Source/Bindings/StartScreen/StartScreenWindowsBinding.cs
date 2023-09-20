namespace Omega.Kulibin
{
    public sealed class StartScreenWindowsBinding : IInitBinding
    {
        private readonly WindowsOrganizerPresenter _windowsOrganizer;
        private readonly LoadingWindowPresenter _loadingWindow;

        public StartScreenWindowsBinding(
            WindowsOrganizerPresenter windowsOrganizer,
            LoadingWindowPresenter loadingWindow
            )
        {
            _windowsOrganizer = windowsOrganizer;
            _loadingWindow = loadingWindow;
        }

        public void Init()
        {
            _windowsOrganizer.RegisterWindow(_loadingWindow);
        }
    }
}