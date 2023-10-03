namespace MaxFluff.Prototypes
{
    public sealed class StartScreenWindowsBinding : IInitBinding
    {
        private readonly WindowsOrganizerPresenter _windowsOrganizer;
        private readonly LoadingWindowPresenter _loadingWindow;
        private readonly QuitWindowPresenter _quitWindow;

        public StartScreenWindowsBinding(
            WindowsOrganizerPresenter windowsOrganizer,
            LoadingWindowPresenter loadingWindow,
            QuitWindowPresenter quitWindow
            )
        {
            _windowsOrganizer = windowsOrganizer;
            _loadingWindow = loadingWindow;
            _quitWindow = quitWindow;
        }

        public void Init()
        {
            _windowsOrganizer.RegisterWindow(_loadingWindow);
            _windowsOrganizer.RegisterWindow(_quitWindow);
        }
    }
}