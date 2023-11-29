using UnityEngine;

namespace MaxFluff.Prototypes.TBS
{
    public class BoardView : ViewBase
    {
    }

    public class BoardPresenter : PresenterBase<BoardView>
    {
        private readonly Transform[,] _cells = new Transform[7, 10];

        public BoardPresenter(BoardView view) : base(view)
        {
            InitBoard();
        }

        private void InitBoard()
        {
            for (var i = 0; i < _view.transform.childCount; ++i)
            {
                var horizontal = _view.transform.GetChild(i);
                for (var j = 0; j < horizontal.childCount; ++j)
                {
                    var cell = horizontal.GetChild(j);
                    _cells[i, j] = cell;
                }
            }
        }
    }
}