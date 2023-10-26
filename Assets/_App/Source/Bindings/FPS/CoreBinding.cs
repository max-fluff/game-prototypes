using System;
using MaxFluff.Prototypes.FPS;

namespace MaxFluff.Prototypes
{
    public class CoreBinding : IInitBinding, IStateChangerBinding
    {
        private readonly CorePresenter _corePresenter;
        private readonly WinWindowPresenter _winWindow;
        private readonly CoreVisualPresenter _coreVisual;
        private readonly CursorService _cursorService;

        private int _burnPiecesAmount = 0;
        private int _piecesAmount;

        public CoreBinding(
            CorePresenter corePresenter,
            WinWindowPresenter winWindow,
            CoreVisualPresenter coreVisual,
            CursorService cursorService
        )
        {
            _corePresenter = corePresenter;
            _winWindow = winWindow;
            _coreVisual = coreVisual;
            _cursorService = cursorService;
        }

        public void Init()
        {
            _corePresenter.OnZapped += ProcessCoreDestruction;
            _winWindow.OnClosed += Quit;

            _piecesAmount = _corePresenter.GetCorePiecesAmount();
        }

        private void Quit() =>
            OnStateChangeRequested?.Invoke(new StartScreenState());

        private void ProcessCoreDestruction()
        {
            _burnPiecesAmount++;

            _coreVisual.SetCount(_burnPiecesAmount, _piecesAmount);

            if (_burnPiecesAmount >= _piecesAmount)
            {
                _cursorService.IsCursorVisible = true;
                _winWindow.Open();
            }
        }

        public event Action<IAppState> OnStateChangeRequested;
    }
}