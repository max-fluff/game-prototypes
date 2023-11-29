using System;
using System.Collections.Generic;
using UnityEngine;

namespace MaxFluff.Prototypes
{
    public class BoardView : ViewBase
    {
    }

    public class BoardPresenter : PresenterBase<BoardView>
    {
        private readonly Cell[,] _cells = new Cell[7, 10];

        public event Action<int, int, Cell> OnCellClicked;

        private BoardState _state;

        private Side _currentSide;

        private (int x, int y) _currentFigurePos;

        private Figure CurrentFigure
        {
            get
            {
                var x = _currentFigurePos.x;
                var y = _currentFigurePos.y;
                if (x <= _cells.GetUpperBound(0)
                    && x >= _cells.GetLowerBound(0)
                    && y <= _cells.GetUpperBound(1)
                    && y >= _cells.GetLowerBound(1))
                    return _cells[x, y].FigureOccupied;

                return null;
            }
        }

        public BoardState State
        {
            get => _state;

            set
            {
                var cachedState = State;
                if (value == State) return;

                _state = value;

                switch (value)
                {
                    case BoardState.SelectingFigure:
                        HighlightAvailablePieces();
                        _currentFigurePos = (-1, -1);
                        break;
                    case BoardState.Moving:
                        break;
                    case BoardState.Action:
                        foreach (var cellFromList in _cells)
                            cellFromList.Highlight(false);

                        if (!HighLightCellsToAction(_currentFigurePos.x, _currentFigurePos.y,
                                CurrentFigure.ApplyActionForOtherSide)) State = cachedState;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(value), value, null);
                }
            }
        }

        private void HighlightAvailablePieces()
        {
            foreach (var cell in _cells)
            {
                cell.Highlight(cell.State == Prototypes.State.Figure && cell.FigureOccupied.Side == _currentSide);
            }
        }

        public BoardPresenter(BoardView view) : base(view)
        {
            InitBoard();

            State = BoardState.SelectingFigure;
        }

        private void InitBoard()
        {
            for (var i = 0; i < _view.transform.childCount; ++i)
            {
                var horizontal = _view.transform.GetChild(i);
                for (var j = 0; j < horizontal.childCount; ++j)
                {
                    var cell = horizontal.GetChild(j).GetComponent<Cell>();
                    _cells[i, j] = cell;
                    var j1 = j;
                    var i1 = i;
                    cell.OnCellClicked += () =>
                    {
                        ProcessCellClick(i1, j1, cell);
                        OnCellClicked?.Invoke(i1, j1, cell);
                    };
                }
            }
        }

        private void ProcessCellClick(int x, int y, Cell cell)
        {
            switch (_state)
            {
                case BoardState.None:
                    break;
                case BoardState.SelectingFigure:
                    ProcessSelection(x, y, cell);

                    break;
                case BoardState.Moving:
                    var neighborCells = new List<(int x, int y)>
                    {
                        (_currentFigurePos.x - 1, _currentFigurePos.y),
                        (_currentFigurePos.x + 1, _currentFigurePos.y),
                        (_currentFigurePos.x, _currentFigurePos.y + 1),
                        (_currentFigurePos.x, _currentFigurePos.y - 1),
                    };

                    if (neighborCells.Contains((x, y)) && GetCellAvailability(x, y, true))
                    {
                        _cells[x, y].State = Prototypes.State.Figure;
                        _cells[x, y].FigureOccupied = CurrentFigure;
                        _cells[_currentFigurePos.x, _currentFigurePos.y].State = Prototypes.State.None;
                        _currentFigurePos = (x, y);

                        foreach (var cellFromList in _cells)
                            cellFromList.Highlight(false);

                        HighLightCellsToMove(x, y);
                    }
                    else
                    {
                        //check if first move
                        ProcessSelection(x, y, cell);
                    }

                    break;

                case BoardState.Action:
                    foreach (var cellFromList in _cells)
                        cellFromList.Highlight(false);

                    //ProcessAction

                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void ProcessSelection(int x, int y, Cell cell)
        {
            if (cell.State == Prototypes.State.Figure && cell.FigureOccupied.Side == _currentSide)
            {
                foreach (var cellFromList in _cells)
                    cellFromList.Highlight(false);

                if (CurrentFigure)
                    CurrentFigure.SetSelected(false);

                _currentFigurePos = (x, y);
                CurrentFigure.SetSelected(true);
                HighLightCellsToMove(x, y);

                State = BoardState.Moving;
            }
        }

        private void HighLightCellsToMove(int x, int y)
        {
            var cellsToHighlight = new List<(int x, int y)>
            {
                (x - 1, y),
                (x + 1, y),
                (x, y + 1),
                (x, y - 1),
            };

            foreach (var cellToHighlight in cellsToHighlight)
            {
                if (GetCellAvailability(cellToHighlight.x, cellToHighlight.y, true))
                {
                    _cells[cellToHighlight.x, cellToHighlight.y].Highlight(true);
                }
            }
        }

        private bool HighLightCellsToAction(int x, int y, bool highlightForOtherSide)
        {
            var cellsToHighlight = _cells[x, y].FigureOccupied.GetHighlightedAction();
            if (cellsToHighlight is null)
                return false;

            for (var index = 0; index < cellsToHighlight.Count; index++)
            {
                var cell = cellsToHighlight[index];
                cellsToHighlight[index] = (cell.x + x, cell.y + y);
            }

            foreach (var cellToHighlight in cellsToHighlight)
            {
                if (GetCellAvailability(cellToHighlight.x, cellToHighlight.y, highlightForOtherSide))
                {
                    _cells[cellToHighlight.x, cellToHighlight.y].Highlight(true);
                }
            }

            return true;
        }

        private bool GetCellAvailability(int x, int y, bool highlightOverOtherSide)
        {
            if (x <= _cells.GetUpperBound(0)
                && x >= _cells.GetLowerBound(0)
                && y <= _cells.GetUpperBound(1)
                && y >= _cells.GetLowerBound(1))
            {
                var cell = _cells[x, y];
                if (cell.State != Prototypes.State.Figure ||
                    highlightOverOtherSide && cell.FigureOccupied.Side != _currentSide)
                {
                    return true;
                }
            }

            return false;
        }
    }

    public enum BoardState
    {
        None,
        SelectingFigure,
        Moving,
        Action
    }
}