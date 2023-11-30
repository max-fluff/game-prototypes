using System.Collections.Generic;
using System.Linq;

namespace MaxFluff.Prototypes
{
    public class Worker : Figure
    {
        private readonly List<(int x, int y)> _highlightableOnAction = new List<(int x, int y)>
        {
            (-1, 0),
            (1, 0),
            (0, 1),
            (0, -1),
        };

        public override bool ApplyActionForOtherSide => false;

        public override List<(int x, int y)> GetHighlightedAction() => _highlightableOnAction.ToList();
    }
}