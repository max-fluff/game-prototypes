using System.Collections.Generic;
using System.Linq;

namespace MaxFluff.Prototypes
{
    public class Trebuchet : Figure
    {
        private readonly List<(int x, int y)> _highlightableOnAction = new List<(int x, int y)>
        {
            (-3, 0),
            (-4, 0),
            (3, 0),
            (4, 0),
            (0, 3),
            (0, 4),
            (0, -3),
            (0, -4),
        };

        public override int MoveTime => 10;
        public override int ActionTime => 15;
        public override List<(int x, int y)> GetHighlightedAction() => _highlightableOnAction.ToList();
    }
}