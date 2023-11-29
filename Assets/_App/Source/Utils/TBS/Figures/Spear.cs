using System.Collections.Generic;
using UnityEngine;

namespace MaxFluff.Prototypes
{
    public class Spear : Figure
    {
        public override FigureType FigureType => FigureType.Spear;

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

        public override List<(int x, int y)> GetHighlightedAction() => _highlightableOnAction;
    }
}