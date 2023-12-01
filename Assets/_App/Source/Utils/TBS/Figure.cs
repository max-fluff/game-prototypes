using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MaxFluff.Prototypes
{
    public abstract class Figure : MonoBehaviour
    {
        public bool AllActionsSuccessful = true;
        public bool MadeAnyAction;
        public bool IsKilled;
        private bool _isKilledInit = false;

        public (int x, int y) InitPos;

        private readonly List<(int x, int y)> _highlightableOnMovement = new List<(int x, int y)>
        {
            (-1, 0),
            (1, 0),
            (0, 1),
            (0, -1),
        };

        public virtual bool ApplyActionForOtherSide => true;
        public abstract int MoveTime { get; }
        public abstract int ActionTime { get; }

        public Side Side;
        [SerializeField] private GameObject Outline;

        public void SetSelected(bool isSelected) => Outline.SetActive(isSelected);

        public virtual List<(int x, int y)> GetHighlightedAction() => null;
        public virtual List<(int x, int y)> GetHighlightedMovement() => _highlightableOnMovement.ToList();

        public virtual void Reset()
        {
            IsKilled = _isKilledInit;
            gameObject.SetActive(!IsKilled);
            SetSelected(false);
            AllActionsSuccessful = true;
        }

        public void RecordInitState()
        {
            _isKilledInit = IsKilled;
            OnRecordInitState();
        }

        protected virtual void OnRecordInitState()
        {
        }

        public void Kill()
        {
            AllActionsSuccessful = false;
            gameObject.SetActive(false);
            IsKilled = true;
            OnKill();
        }

        protected virtual void OnKill()
        {
        }
    }

    public enum Side
    {
        White,
        Black
    }
}