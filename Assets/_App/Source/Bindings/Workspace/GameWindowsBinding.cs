namespace MaxFluff.Prototypes
{
    public sealed class GameWindowsBinding : IInitBinding
    {
        private readonly WindowsOrganizerPresenter _windowsOrganizer;
        private readonly LoadingWindowPresenter _loadingWindowPresenter;

        public GameWindowsBinding(
            WindowsOrganizerPresenter windowsOrganizer,
            LoadingWindowPresenter loadingWindowPresenter
        )
        {
            _windowsOrganizer = windowsOrganizer;
            _loadingWindowPresenter = loadingWindowPresenter;
        }

        public void Init()
        {
            _windowsOrganizer.RegisterWindow(_loadingWindowPresenter);
        }
    }
}