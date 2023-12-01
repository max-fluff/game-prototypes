using System;
using System.Collections.Generic;
using System.Linq;

namespace MaxFluff.Prototypes
{
    public class Spear : Figure
    {
        public List<(int x, int y)> usedCells = new List<(int x, int y)>();
        private List<(int x, int y)> _usedCellsInit = new List<(int x, int y)>();

        public event Action<Spear> OnKilled;

        private readonly List<(int x, int y)> _highlightableOnAction = new List<(int x, int y)>
        {
            (-1, 0),
            (1, 0),
            (0, 1),
            (0, -1),
            (-2, 0),
            (2, 0),
            (0, 2),
            (0, -2),
        };

        public override int MoveTime => 5;
        public override int ActionTime => 5;

        public override List<(int x, int y)> GetHighlightedAction()
        {
            return isStationed ? new List<(int x, int y)> { (0, 0) } : _highlightableOnAction.ToList();
        }

        public override List<(int x, int y)> GetHighlightedMovement() =>
            isStationed ? null : base.GetHighlightedMovement();


        public bool isStationed;
        private bool _isStationedInit = false;

        public override void Reset()
        {
            base.Reset();
            isStationed = _isStationedInit;
            usedCells = _usedCellsInit.ToList();
        }

        protected override void OnRecordInitState()
        {
            _isStationedInit = isStationed;
            _usedCellsInit = usedCells.ToList();
        }

        protected override void OnKill() => OnKilled?.Invoke(this);
    }
}