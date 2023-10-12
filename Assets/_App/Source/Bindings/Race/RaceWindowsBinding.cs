namespace MaxFluff.Prototypes
{
    public sealed class RaceWindowsBinding : IInitBinding
    {
        private readonly WindowsOrganizerPresenter _windowsOrganizer;
        private readonly FailWindowPresenter _failWindow;
        private readonly TimeResultWindowPresenter _timeResultWindow;

        public RaceWindowsBinding(
            WindowsOrganizerPresenter windowsOrganizer,
            FailWindowPresenter failWindow,
            TimeResultWindowPresenter timeResultWindow
        )
        {
            _windowsOrganizer = windowsOrganizer;
            _failWindow = failWindow;
            _timeResultWindow = timeResultWindow;
        }

        public void Init()
        {
            _windowsOrganizer.RegisterWindow(_failWindow);
            _windowsOrganizer.RegisterWindow(_timeResultWindow);
        }
    }
}