namespace MaxFluff.Prototypes
{
    public sealed class FPSWindowsBinding : IInitBinding
    {
        private readonly WindowsOrganizerPresenter _windowsOrganizer;
        private readonly FailWindowPresenter _failWindow;
        private readonly WinWindowPresenter _winWindow;

        public FPSWindowsBinding(
            WindowsOrganizerPresenter windowsOrganizer,
            FailWindowPresenter failWindow,
            WinWindowPresenter winWindow)
        {
            _windowsOrganizer = windowsOrganizer;
            _failWindow = failWindow;
            _winWindow = winWindow;
        }

        public void Init()
        {
            _windowsOrganizer.RegisterWindow(_failWindow);
            _windowsOrganizer.RegisterWindow(_winWindow);
        }
    }
}