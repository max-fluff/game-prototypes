using Cysharp.Threading.Tasks;

namespace MaxFluff.Prototypes
{
    public class PuzzleSheetBinding : IInitBinding
    {
        private readonly SheetView _sheetView;
        private readonly SendButtonPresenter _sendButtonPresenter;
        private readonly DataSheetsForPuzzle _dataSheets;
        private readonly PhonePresenter _phone;

        private int _currentSheet = 0;
        private SheetPresenter _sheetPresenter;

        public PuzzleSheetBinding(
            SheetView sheetView,
            SendButtonPresenter sendButtonPresenter,
            DataSheetsForPuzzle dataSheets,
            PhonePresenter phone
        )
        {
            _sheetView = sheetView;
            _sendButtonPresenter = sendButtonPresenter;
            _dataSheets = dataSheets;
            _phone = phone;
        }

        public void Init()
        {
            NextSheet();

            _sendButtonPresenter.OnClick += Send;
        }

        private void Send()
        {
            _phone.HangUp();

            SendAsync().Forget();
        }

        private async UniTask SendAsync()
        {
            _phone.HangUp();
            _sendButtonPresenter.SetButtonActive(false);

            await _sheetPresenter.Disappear();

            NextSheet();
            await UniTask.Delay(500);
            _sendButtonPresenter.SetButtonActive(true);
        }

        private void NextSheet()
        {
            //_currentSheet++;
            _sheetPresenter = new SheetPresenter(_sheetView, _dataSheets.DataSheets[_currentSheet]);
        }
    }
}