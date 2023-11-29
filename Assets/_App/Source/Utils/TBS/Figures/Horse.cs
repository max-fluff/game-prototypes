using System.Collections.Generic;

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

        public override FigureType FigureType => FigureType.Horse;

        public override List<(int x, int y)> GetHighlightedAction() => _highlightableOnAction;
    }
}