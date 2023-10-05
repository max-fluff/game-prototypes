using Cysharp.Threading.Tasks;

namespace MaxFluff.Prototypes
{
    public class PuzzleSheetBinding : IInitBinding
    {
        private readonly SheetPresenter _sheetPresenter;
        private readonly SendButtonPresenter _sendButtonPresenter;
        private readonly DataSheetsForPuzzle _dataSheets;
        private readonly PhonePresenter _phone;

        private int _currentSheet = -1;

        public PuzzleSheetBinding(
            SheetPresenter sheetPresenter,
            SendButtonPresenter sendButtonPresenter,
            DataSheetsForPuzzle dataSheets,
            PhonePresenter phone
        )
        {
            _sheetPresenter = sheetPresenter;
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
            SendAsync().Forget();
        }

        private async UniTask SendAsync()
        {
            _phone.HangUp();
            _sendButtonPresenter.SetButtonActive(false);

            _sheetPresenter.ShowResult();
            
            await UniTask.Delay(600);

            await _sheetPresenter.Disappear();

            if (_dataSheets.DataSheets.Count - 1 <= _currentSheet)
            {
                _sheetPresenter.ShowCongrats();
                return;
            }

            NextSheet();
            await UniTask.Delay(500);
            _sendButtonPresenter.SetButtonActive(true);
        }

        private void NextSheet()
        {
            _currentSheet++;
            _sheetPresenter.UpdateDataSheet(_dataSheets.DataSheets[_currentSheet]);
        }
    }
}