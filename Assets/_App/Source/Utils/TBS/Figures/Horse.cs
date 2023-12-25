using System.Collections.Generic;
using System.Linq;

namespace MaxFluff.Prototypes
{
    public class Horse : Figure
    {
        private readonly List<(int x, int y)> _highlightableOnAction = new List<(int x, int y)>
        {
            (-2, 0),
            (2, 0),
            (0, 2),
            (0, -2),
        };

        public override bool ApplyActionForOtherSide => true;
        public override int MoveTime => 3;
        public override int ActionTime => 9;

        public override List<(int x, int y)> GetHighlightedAction() => _highlightableOnAction.ToList();
    }
}