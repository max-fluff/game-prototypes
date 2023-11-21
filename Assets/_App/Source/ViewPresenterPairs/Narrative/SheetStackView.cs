using UnityEngine;

namespace MaxFluff.Prototypes
{
    public class SheetStackView : ViewBase
    {
        public TapProcessor TapProcessor;
        public SingleSheetView SingleSheetTemplate;
    }

    public class SheetStackPresenter : PresenterBase<SheetStackView>
    {
        private SingleSheetPresenter _currentSheet;

        public SheetStackPresenter(SheetStackView view) : base(view)
        {
            _view.TapProcessor.OnDraggedRight.AddListener(CreateSheet);
        }

        private void CreateSheet()
        {
            if (_currentSheet != null) return;

            var newSheet = Object.Instantiate(_view.SingleSheetTemplate);
            newSheet.transform.position = _view.transform.position;
            _currentSheet = new SingleSheetPresenter(newSheet);

            _currentSheet.OnDestroy += RemoveCurrentSheet;
            _currentSheet.PlayGetTransition();
            _view.TapProcessor.enabled = false;
        }

        private void RemoveCurrentSheet()
        {
            _currentSheet = null;
            _view.TapProcessor.enabled = true;
        }

        public void SetStampOnCurrent() => _currentSheet.SetStamp();
        public void SetSignatureOnCurrent() => _currentSheet.SetSignature();
    }
}