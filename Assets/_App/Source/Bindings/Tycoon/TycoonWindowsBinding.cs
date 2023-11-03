namespace MaxFluff.Prototypes
{
    public sealed class TycoonWindowsBinding : IInitBinding
    {
        private readonly WindowsOrganizerPresenter _windowsOrganizer;
        private readonly TycoonBarWindowPresenter _barWindow;
        private readonly TycoonResultsWindowPresenter _resultsWindow;
        private readonly TycoonSellWindowPresenter _sellWindow;
        private readonly TycoonGamePlaceWindowPresenter _gamePlaceWindow;
        private readonly FailWindowPresenter _failWindow;

        public TycoonWindowsBinding(
            WindowsOrganizerPresenter windowsOrganizer,
            TycoonBarWindowPresenter barWindow,
            TycoonResultsWindowPresenter resultsWindow,
            TycoonSellWindowPresenter sellWindow,
            TycoonGamePlaceWindowPresenter gamePlaceWindow,
            FailWindowPresenter failWindow
        )
        {
            _windowsOrganizer = windowsOrganizer;
            _barWindow = barWindow;
            _resultsWindow = resultsWindow;
            _sellWindow = sellWindow;
            _gamePlaceWindow = gamePlaceWindow;
            _failWindow = failWindow;
        }

        public void Init()
        {
            _windowsOrganizer.RegisterWindow(_barWindow);
            _windowsOrganizer.RegisterWindow(_resultsWindow);
            _windowsOrganizer.RegisterWindow(_sellWindow);
            _windowsOrganizer.RegisterWindow(_gamePlaceWindow);
            _windowsOrganizer.RegisterWindow(_failWindow);
        }
    }
}