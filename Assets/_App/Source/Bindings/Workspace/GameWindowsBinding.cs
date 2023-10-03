namespace MaxFluff.Prototypes
{
    public sealed class GameWindowsBinding : IInitBinding
    {
        private readonly WindowsOrganizerPresenter _windowsOrganizer;
        private readonly LoadingWindowPresenter _loadingWindowPresenter;
        private readonly QuitWindowPresenter _quitWindowPresenter;

        public GameWindowsBinding(
            WindowsOrganizerPresenter windowsOrganizer,
            LoadingWindowPresenter loadingWindowPresenter,
            QuitWindowPresenter quitWindowPresenter
        )
        {
            _windowsOrganizer = windowsOrganizer;
            _loadingWindowPresenter = loadingWindowPresenter;
            _quitWindowPresenter = quitWindowPresenter;
        }

        public void Init()
        {
            _windowsOrganizer.RegisterWindow(_loadingWindowPresenter);
            _windowsOrganizer.RegisterWindow(_quitWindowPresenter);
        }
    }
}