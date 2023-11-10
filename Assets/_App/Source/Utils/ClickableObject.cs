using System;
using UnityEngine;

namespace MaxFluff.Prototypes
{
    public class ClickableObject : MonoBehaviour
    {
        public event Action OnClicked;
        public event Action OnHover;
        public event Action OnUnhover;

        private void OnMouseDown()
        {
            OnClicked?.Invoke();
        }

        private void OnMouseOver() => OnHover?.Invoke();
        private void OnMouseExit() => OnUnhover?.Invoke();
    }
}