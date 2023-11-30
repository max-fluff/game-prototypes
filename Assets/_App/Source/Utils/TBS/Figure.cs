using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MaxFluff.Prototypes
{
    public abstract class Figure : MonoBehaviour
    {
        private readonly List<(int x, int y)> _highlightableOnMovement = new List<(int x, int y)>
        {
            (-1, 0),
            (1, 0),
            (0, 1),
            (0, -1),
        };

        public virtual bool ApplyActionForOtherSide => true;

        public Side Side;
        [SerializeField] private GameObject Outline;

        public void SetSelected(bool isSelected) => Outline.SetActive(isSelected);

        public virtual List<(int x, int y)> GetHighlightedAction() => null;
        public virtual List<(int x, int y)> GetHighlightedMovement() => _highlightableOnMovement.ToList();

        public virtual void Reset()
        {
            gameObject.SetActive(true);
            SetSelected(false);
        }

        public void Kill()
        {
            gameObject.SetActive(false);
        }
    }

    public enum FigureType
    {
        Rook,
        Horse,
        Spear,
        Worker,
        Trebuchet,
        King
    }

    public enum Side
    {
        White,
        Black
    }
}