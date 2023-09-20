namespace MaxFluff.Prototypes
{
    public sealed class WorkspaceWindowsBinding : IInitBinding
    {
        private readonly WindowsOrganizerPresenter _windowsOrganizer;
        private readonly SettingsWindowPresenter _settingsWindow;
        private readonly LoadingWindowPresenter _loadingWindowPresenter;

        public WorkspaceWindowsBinding(
            WindowsOrganizerPresenter windowsOrganizer,
            SettingsWindowPresenter settingsWindow,
            LoadingWindowPresenter loadingWindowPresenter
        )
        {
            _windowsOrganizer = windowsOrganizer;
            _settingsWindow = settingsWindow;
            _loadingWindowPresenter = loadingWindowPresenter;
        }

        public void Init()
        {
            _windowsOrganizer.RegisterWindow(_settingsWindow);
            _windowsOrganizer.RegisterWindow(_loadingWindowPresenter);
        }
    }
}