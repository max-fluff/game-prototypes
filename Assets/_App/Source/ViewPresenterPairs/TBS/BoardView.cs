using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Lean.Gui;
using UnityEngine;
using UnityEngine.UI;

namespace MaxFluff.Prototypes
{
    public class BoardView : ViewBase
    {
        public GameObject Cover;
        public Slider Progress;
        public LeanButton SkipStepButton;
        public LeanButton SkipSkipToEndButton;
    }

    public class BoardPresenter : PresenterBase<BoardView>
    {
        private readonly Cell[,] _cells = new Cell[7, 10];

        private readonly List<Figure> _figures = new List<Figure>();

        private readonly Dictionary<int, List<RecordedAction>> _recordedActions =
            new Dictionary<int, List<RecordedAction>>();

        private BoardState _state;

        private Side _currentSide;

        private int _step;
        private const int STEP_TIME_MILLISECONDS = 100;
        private const int MAX_STEP = 30;

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

        public int Step
        {
            get => _step;
            set
            {
                _view.Progress.value = value;
                _step = value;
            }
        }

        public async UniTask SkipSteps(int turnsAmount)
        {
            var figure = CurrentFigure;
            _view.Cover.SetActive(true);
            for (var i = 0; i < turnsAmount; i++)
            {
                Step++;

                if (_recordedActions.TryGetValue(Step, out var action))
                    foreach (var recordedAction in action)
                    {
                        RestoreAction(recordedAction);
                        await UniTask.Delay(STEP_TIME_MILLISECONDS);
                    }
                else
                    await UniTask.Delay(STEP_TIME_MILLISECONDS);

                if (!figure || figure.IsKilled)
                {
                    turnsAmount = MAX_STEP - Step;
                    i = 0;
                }

                if (Step == MAX_STEP - 1)
                {
                    await NextTurn();
                    break;
                }
            }

            _view.Cover.SetActive(false);
        }

        private async UniTask NextTurn()
        {
            foreach (var cell in _cells)
            {
                cell.State = Prototypes.State.None;
            }

            foreach (var figure in _figures)
            {
                figure.Reset();
                _cells[figure.InitPos.x, figure.InitPos.y].FigureOccupied = figure;
                _cells[figure.InitPos.x, figure.InitPos.y].State = Prototypes.State.Figure;
            }

            Step = 0;

            if (_recordedActions.TryGetValue(Step, out var action))
                foreach (var recordedAction in action)
                {
                    RestoreAction(recordedAction);
                    await UniTask.Delay(STEP_TIME_MILLISECONDS);
                }

            _currentSide = _currentSide switch
            {
                Side.Black => Side.White,
                Side.White => Side.Black,
                _ => _currentSide
            };

            State = BoardState.SelectingFigure;
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

            _view.SkipStepButton.OnClick.AddListener(() => SkipSteps(1).Forget());
            _view.SkipSkipToEndButton.OnClick.AddListener(() => SkipSteps(MAX_STEP - Step).Forget());
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
                    cell.OnCellClicked += () => { ProcessCellClick(i1, j1, cell); };

                    var figure = cell.GetComponentInChildren<Figure>();
                    if (figure)
                    {
                        figure.InitPos = (i, j);
                        _figures.Add(figure);
                    }
                }
            }
        }

        private void ProcessCellClick(int x, int y, Cell cell)
        {
            ProcessCellClickAsync(x, y, cell).Forget();
        }

        private async UniTask ProcessCellClickAsync(int x, int y, Cell cell)
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

                    if (cellsToHighlight.Contains((x, y)) && GetCellAvailability(CurrentFigure, x, y, true))
                    {
                        var currentFigure = CurrentFigure;
                        var success = MoveToNewPlace(currentFigure, x, y);
                        _currentFigurePos = (x, y);

                        RecordAction(currentFigure, RecordedActionType.Moving, x, y);

                        if (!success)
                        {
                            await SkipSteps(MAX_STEP - Step);
                            return;
                        }

                        foreach (var cellFromList in _cells)
                            cellFromList.Highlight(false);

                        var cachedSide1 = _currentSide;
                        await SkipSteps(CurrentFigure.MoveTime);
                        if (_currentSide != cachedSide1)
                            return;

                        HighLightCellsToMove(x, y);
                    }
                    else
                    {
                        ProcessSelection(x, y, cell);
                    }

                    break;

                case BoardState.Action:
                    var cachedSide2 = _currentSide;

                    await Action(x, y);

                    if (_currentSide != cachedSide2)
                        return;

                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private bool MoveToNewPlace(Figure figure, int x, int y)
        {
            var (figX, figY) = GetFigureCoord(figure);

            if (_cells[x, y].State == Prototypes.State.Figure || _cells[x, y].State == Prototypes.State.Spear)
            {
                figure.Kill();
                _cells[figX, figY].ToLastState();

                if (_cells[x, y].State == Prototypes.State.Figure &&
                    _cells[x, y].FigureOccupied.Side != figure.Side)
                {
                    _cells[x, y].FigureOccupied.Kill();
                    _cells[x, y].ToLastState();
                }

                return false;
            }

            _cells[x, y].State = Prototypes.State.Figure;
            _cells[x, y].FigureOccupied = figure;
            _cells[figX, figY].ToLastState();

            return true;
        }

        private async UniTask Action(int x, int y)
        {
            var cellsToHighlight = CurrentFigure.GetHighlightedAction();
            for (var index = 0; index < cellsToHighlight.Count; index++)
            {
                var cell = cellsToHighlight[index];
                cellsToHighlight[index] = (cell.x + _currentFigurePos.x, cell.y + _currentFigurePos.y);
            }

            if (GetCellAvailability(CurrentFigure, x, y, CurrentFigure.ApplyActionForOtherSide) &&
                cellsToHighlight.Contains((x, y)))
            {
                switch (CurrentFigure)
                {
                    case Horse horse:

                        var moved = MoveToNewPlace(horse, x, y);
                        _currentFigurePos = (x, y);

                        RecordAction(horse, RecordedActionType.Moving, x, y);

                        if (!moved)
                        {
                            await SkipSteps(MAX_STEP - Step);
                            return;
                        }

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
                                    if (ProcessSpearCell(newX, newY, x, y, spear))
                                        RecordAction(spear, RecordedActionType.Action, x, y);
                                    else
                                        return;
                                }
                                else
                                {
                                    var newX = x + x - _currentFigurePos.x;
                                    var newY = y + y - _currentFigurePos.y;

                                    if (ProcessSpearCell(x, y, newX, newY, spear))
                                        RecordAction(spear, RecordedActionType.Action, x, y);
                                    else
                                        return;
                                }

                                spear.isStationed = !spear.isStationed;
                            }
                        }

                        break;
                    case Trebuchet trebuchet:

                        if (GetCellAvailability(CurrentFigure, x, y, CurrentFigure.ApplyActionForOtherSide))
                        {
                            RecordAction(trebuchet, RecordedActionType.Action, x, y);

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
                        if (GetCellAvailability(CurrentFigure, x, y, CurrentFigure.ApplyActionForOtherSide))
                        {
                            RecordAction(worker, RecordedActionType.Action, x, y);

                            _cells[x, y].State = Prototypes.State.Barricade;
                        }

                        break;
                }
            }
            else
                return;

            var cachedSide2 = _currentSide;

            await SkipSteps(CurrentFigure.ActionTime);

            if (_currentSide != cachedSide2)
                return;

            foreach (var cellFromList in _cells)
                cellFromList.Highlight(false);

            ProcessSelection(_currentFigurePos.x, _currentFigurePos.y,
                _cells[_currentFigurePos.x, _currentFigurePos.y]);
            State = BoardState.Moving;
        }

        private bool ProcessSpearCell(int x1, int y1, int x2, int y2, Spear spear)
        {
            if (GetCellAvailability(spear, x1, y1, spear.ApplyActionForOtherSide))
            {
                MakeCellSpeared(spear, x1, y1);

                if (GetCellAvailability(spear, x2, y2, spear.ApplyActionForOtherSide))
                    MakeCellSpeared(spear, x2, y2);
            }
            else
            {
                return false;
            }

            return true;
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
                if (GetCellAvailability(CurrentFigure, cellToHighlight.x, cellToHighlight.y, true))
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
                if (GetCellAvailability(CurrentFigure, cellToHighlight.x, cellToHighlight.y, highlightForOtherSide))
                {
                    _cells[cellToHighlight.x, cellToHighlight.y].Highlight(true);
                }
            }

            return true;
        }

        private bool GetCellAvailability(Figure figure, int x, int y, bool highlightOverOtherSide)
        {
            if (x <= _cells.GetUpperBound(0)
                && x >= _cells.GetLowerBound(0)
                && y <= _cells.GetUpperBound(1)
                && y >= _cells.GetLowerBound(1))
            {
                var cell = _cells[x, y];

                if ((cell.State != Prototypes.State.Figure && cell.State != Prototypes.State.Barricade)
                    || highlightOverOtherSide && cell.State == Prototypes.State.Figure &&
                    cell.FigureOccupied.Side != figure.Side
                    || (cell.State == Prototypes.State.Figure && cell.FigureOccupied == figure))
                {
                    return true;
                }
            }

            return false;
        }

        private void RecordAction(Figure figure, RecordedActionType type, int x, int y)
        {
            if (!_recordedActions.ContainsKey(Step))
                _recordedActions.Add(Step, new List<RecordedAction>());

            _recordedActions[Step].Add(new RecordedAction
            {
                Type = type,
                Coordinate = (x, y),
                Figure = figure
            });

            figure.MadeAnyAction = true;
        }

        private void RestoreAction(RecordedAction action)
        {
            if (!action.Figure.AllActionsSuccessful)
                return;

            var (figX, figY) = GetFigureCoord(action.Figure);

            var x = action.Coordinate.x;
            var y = action.Coordinate.y;
            var actionSuccessful = true;

            switch (action.Type)
            {
                case RecordedActionType.Moving:
                    var cellsToHighlight = action.Figure.GetHighlightedMovement();

                    for (var index = 0; index < cellsToHighlight.Count; index++)
                    {
                        var cellToHighlight = cellsToHighlight[index];
                        cellsToHighlight[index] = (cellToHighlight.x + figX,
                            cellToHighlight.y + figY);
                    }

                    if (cellsToHighlight.Contains((x, y)) &&
                        GetCellAvailability(action.Figure, x, y, true))
                        actionSuccessful = MoveToNewPlace(action.Figure, x, y);
                    else
                        actionSuccessful = false;

                    break;
                case RecordedActionType.Action:
                    switch (action.Figure)
                    {
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
                                    if (Mathf.Abs(x - figX) > 1 ||
                                        Mathf.Abs(y - figY) > 1)
                                    {
                                        var newX = (x + figX) / 2;
                                        var newY = (y + figY) / 2;
                                        if (!ProcessSpearCell(newX, newY, x, y, spear))
                                        {
                                            actionSuccessful = false;
                                            break;
                                        }
                                    }
                                    else
                                    {
                                        var newX = x + x - figX;
                                        var newY = y + y - figY;

                                        if (!ProcessSpearCell(x, y, newX, newY, spear))
                                        {
                                            actionSuccessful = false;
                                            break;
                                        }
                                    }

                                    spear.isStationed = !spear.isStationed;
                                }
                            }

                            break;
                        case Trebuchet trebuchet:
                            if (GetCellAvailability(action.Figure, x, y, action.Figure.ApplyActionForOtherSide))
                            {
                                if (_cells[x, y].State == Prototypes.State.Barricade)
                                    _cells[x, y].State = Prototypes.State.None;

                                if (_cells[x, y].State == Prototypes.State.Figure)
                                {
                                    _cells[x, y].FigureOccupied.Kill();
                                    _cells[x, y].ToLastState();
                                }
                            }
                            else
                                actionSuccessful = false;

                            break;
                        case Worker worker:
                            if (GetCellAvailability(action.Figure, x, y, action.Figure.ApplyActionForOtherSide))
                                _cells[x, y].State = Prototypes.State.Barricade;
                            else
                                actionSuccessful = false;

                            break;
                    }

                    action.Figure.AllActionsSuccessful = actionSuccessful;

                    break;
            }
        }

        private (int x, int y) GetFigureCoord(Figure figure)
        {
            for (var i = 0; i <= _cells.GetUpperBound(0); i++)
            {
                for (var j = 0; j <= _cells.GetUpperBound(1); j++)
                {
                    if (_cells[i, j].FigureOccupied == figure)
                    {
                        return (i, j);
                    }
                }
            }

            return (-1, -1);
        }
    }

    public enum BoardState
    {
        None,
        SelectingFigure,
        Moving,
        Action
    }

    public enum RecordedActionType
    {
        Moving,
        Action
    }

    internal struct RecordedAction
    {
        public Figure Figure;
        public RecordedActionType Type;
        public (int x, int y) Coordinate;
    }
}