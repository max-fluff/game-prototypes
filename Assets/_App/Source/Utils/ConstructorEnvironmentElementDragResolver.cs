using UnityEngine;
using UnityEngine.EventSystems;

namespace Omega.Kulibin
{
    public class ConstructorEnvironmentElementDragResolver : DragDropTargetResolver
    {
        private void OnMouseDown()
        {
            if (Input.GetMouseButtonDown(0))
                OnBeginDrag(new PointerEventData(EventSystem.current));
        }

        private void OnMouseUp()
        {
            if (Input.GetMouseButtonUp(0))
                OnEndDrag(new PointerEventData(EventSystem.current));
        }
    }
}