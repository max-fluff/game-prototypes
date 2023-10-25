using System;
using System.Collections.Generic;

namespace MaxFluff.Prototypes.FPS
{
    public class CoreView : ViewBase
    {
        public List<CorePieceView> CorePieces;
    }

    public class CorePresenter : PresenterBase<CoreView>
    {
        private List<CorePiecePresenter> _corePieces = new List<CorePiecePresenter>();

        public event Action OnZapped;

        public CorePresenter(CoreView view) : base(view)
        {
            foreach (var corePiece in _view.CorePieces)
            {
                var presenter = new CorePiecePresenter(corePiece);
                _corePieces.Add(presenter);

                presenter.OnZapped += SendOnZapped;
            }
        }

        public int GetCorePiecesAmount() =>
            _corePieces.Count;

        private void SendOnZapped() =>
            OnZapped?.Invoke();
    }
}