using System.Collections.Generic;
using UnityEngine;

namespace MaxFluff.Prototypes
{
    public abstract class Figure : MonoBehaviour
    {
        public abstract FigureType FigureType { get; }
        public virtual bool ApplyActionForOtherSide => true;

        public Side Side;
        [SerializeField] private GameObject Outline;

        public void SetSelected(bool isSelected) => Outline.SetActive(isSelected);

        public virtual List<(int x, int y)> GetHighlightedAction() => null;
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