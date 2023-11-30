using System;
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
                        foreach (var cellFromList in _cells)
                            cellFromList.Highlight(false);

                        HighLightCellsToMove(_currentFigurePos.x, _currentFigurePos.y);
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
                    var cellsToHighlight = CurrentFigure.GetHighlightedMovement();

                    if (cellsToHighlight is null)
                    {
                        ProcessSelection(x, y, cell);
                        return;
                    }

                    for (var index = 0; index < cellsToHighlight.Count; index++)
                    {
                        var cellToHighlight = cellsToHighlight[index];
                        cellsToHighlight[index] = (cellToHighlight.x + _currentFigurePos.x,
                            cellToHighlight.y + _currentFigurePos.y);
                    }

                    if (cellsToHighlight.Contains((x, y)) && GetCellAvailability(x, y, true))
                    {
                        MoveToNewPlace(x, y);

                        if (_state != BoardState.Moving) break;

                        foreach (var cellFromList in _cells)
                            cellFromList.Highlight(false);

                        HighLightCellsToMove(x, y);
                    }
                    else
                    {
                        ProcessSelection(x, y, cell);
                    }

                    break;

                case BoardState.Action:
                    Action(x, y);

                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void MoveToNewPlace(int x, int y)
        {
            if (_cells[x, y].State == Prototypes.State.Figure || _cells[x, y].State == Prototypes.State.Spear)
            {
                _cells[_currentFigurePos.x, _currentFigurePos.y].FigureOccupied.Kill();
                _cells[_currentFigurePos.x, _currentFigurePos.y].ToLastState();

                State = BoardState.SelectingFigure; //todo fix

                if (_cells[x, y].State == Prototypes.State.Figure &&
                    _cells[x, y].FigureOccupied.Side != _currentSide)
                {
                    _cells[x, y].FigureOccupied.Kill();
                    _cells[x, y].ToLastState();
                }

                return;
            }

            _cells[x, y].State = Prototypes.State.Figure;
            _cells[x, y].FigureOccupied = CurrentFigure;
            _cells[_currentFigurePos.x, _currentFigurePos.y].ToLastState();
            _currentFigurePos = (x, y);
        }

        private void Action(int x, int y)
        {
            var cellsToHighlight = CurrentFigure.GetHighlightedAction();
            for (var index = 0; index < cellsToHighlight.Count; index++)
            {
                var cell = cellsToHighlight[index];
                cellsToHighlight[index] = (cell.x + _currentFigurePos.x, cell.y + _currentFigurePos.y);
            }

            if (GetCellAvailability(x, y, CurrentFigure.ApplyActionForOtherSide) &&
                cellsToHighlight.Contains((x, y)))
            {
                switch (CurrentFigure)
                {
                    case Horse horse:

                        MoveToNewPlace(x, y);

                        break;
                    case Spear spear:
                        if (spear.isStationed)
                        {
                            foreach (var usedCell in spear.usedCells)
                            {
                                _cells[usedCell.x, usedCell.y].State = Prototypes.State.None;
                            }

                            spear.isStationed = !spear.isStationed;

                            spear.usedCells.Clear();
                        }
                        else
                        {
                            if (_cells[x, y].State == Prototypes.State.None ||
                                _cells[x, y].State == Prototypes.State.Figure)
                            {
                                if (Mathf.Abs(x - _currentFigurePos.x) > 1 ||
                                    Mathf.Abs(y - _currentFigurePos.y) > 1)
                                {
                                    var newX = (x + _currentFigurePos.x) / 2;
                                    var newY = (y + _currentFigurePos.y) / 2;
                                    if (GetCellAvailability(newX, newY, CurrentFigure.ApplyActionForOtherSide))
                                    {
                                        MakeCellSpeared(spear, newX, newY);

                                        if (GetCellAvailability(x, y, CurrentFigure.ApplyActionForOtherSide))
                                            MakeCellSpeared(spear, x, y);
                                    }
                                    else
                                    {
                                        return;
                                    }
                                }
                                else
                                {
                                    if (GetCellAvailability(x, y, CurrentFigure.ApplyActionForOtherSide))
                                    {
                                        MakeCellSpeared(spear, x, y);

                                        var newX = x + x - _currentFigurePos.x;
                                        var newY = y + y - _currentFigurePos.y;
                                        if (GetCellAvailability(newX, newY, CurrentFigure.ApplyActionForOtherSide))
                                            MakeCellSpeared(spear, newX, newY);
                                    }
                                    else
                                    {
                                        return;
                                    }
                                }

                                spear.isStationed = !spear.isStationed;
                            }
                        }

                        break;
                    case Trebuchet trebuchet:

                        if (GetCellAvailability(x, y, CurrentFigure.ApplyActionForOtherSide))
                        {
                            if (_cells[x, y].State == Prototypes.State.Barricade)
                                _cells[x, y].State = Prototypes.State.None;

                            if (_cells[x, y].State == Prototypes.State.Figure)
                            {
                                _cells[x, y].FigureOccupied.Kill();
                                _cells[x, y].ToLastState();
                            }
                        }

                        break;
                    case Worker worker:
                        if (GetCellAvailability(x, y, CurrentFigure.ApplyActionForOtherSide))
                        {
                            _cells[x, y].State = Prototypes.State.Barricade;
                        }

                        break;
                }
            }
            else
                return;

            foreach (var cellFromList in _cells)
                cellFromList.Highlight(false);
            ProcessSelection(_currentFigurePos.x, _currentFigurePos.y,
                _cells[_currentFigurePos.x, _currentFigurePos.y]);
            State = BoardState.Moving;
        }

        private void MakeCellSpeared(Spear spear, int x, int y)
        {
            if (_cells[x, y].State == Prototypes.State.Figure &&
                _cells[x, y].FigureOccupied.Side != _currentSide)
            {
                _cells[x, y].FigureOccupied.Kill();
                _cells[x, y].ToLastState();
            }

            _cells[x, y].State = Prototypes.State.Spear;
            _cells[x, y].SpearSide = _currentSide;

            spear.usedCells.Add((x, y));
        }

        private void ProcessSelection(int x, int y, Cell cell)
        {
            //todo: check if moved
            if (cell.State == Prototypes.State.Figure && cell.FigureOccupied.Side == _currentSide)
            {
                foreach (var cellFromList in _cells)
                    cellFromList.Highlight(false);

                if (CurrentFigure)
                    CurrentFigure.SetSelected(false);

                _currentFigurePos = (x, y);
                CurrentFigure.SetSelected(true);

                State = BoardState.Moving;

                HighLightCellsToMove(_currentFigurePos.x, _currentFigurePos.y);
            }
        }

        private void HighLightCellsToMove(int x, int y)
        {
            var cellsToHighlight = _cells[x, y].FigureOccupied.GetHighlightedMovement();
            if (cellsToHighlight is null)
                return;

            for (var index = 0; index < cellsToHighlight.Count; index++)
            {
                var cell = cellsToHighlight[index];
                cellsToHighlight[index] = (cell.x + x, cell.y + y);
            }

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

                if ((cell.State != Prototypes.State.Figure && cell.State != Prototypes.State.Barricade)
                    || highlightOverOtherSide && cell.State == Prototypes.State.Figure &&
                    cell.FigureOccupied.Side != _currentSide
                    || (cell.State == Prototypes.State.Figure && cell.FigureOccupied == CurrentFigure))
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