using System;
using UnityEngine.EventSystems;

namespace CodeBase.Hands
{
    public class DragableHand : Hand, IDragHandler, IEndDragHandler
    {
        public event Action BeingDrag;
        public event Action EndDrag;
        
        public void OnDrag(PointerEventData eventData)
        {
            BeingDrag?.Invoke();
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            EndDrag?.Invoke();
        }
    }
}