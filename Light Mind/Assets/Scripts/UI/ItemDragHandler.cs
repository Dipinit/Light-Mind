using UnityEngine;
using UnityEngine.EventSystems;

namespace UI
{
    public class ItemDragHandler : MonoBehaviour, IDragHandler, IDropHandler
    {
        public void OnDrag(PointerEventData eventData)
        {
            transform.position = Input.mousePosition;
        }

        public void OnDrop(PointerEventData eventData)
        {
            transform.localPosition = Vector3.zero;
        }
    }
}