using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Omega.Kulibin
{
    public class DragDropTargetResolver : MonoBehaviour, IPointerClickHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
    {
        public event Action OnClick;
        public event Action OnDragStart;
        public event Action OnDragEnd;

        public void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Left)
                OnClick?.Invoke();
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Left)
                OnDragStart?.Invoke();
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Left)
                OnDragEnd?.Invoke();
        }

        public void OnDrag(PointerEventData eventData) {}
    }
}