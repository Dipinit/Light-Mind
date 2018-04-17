using System.Linq;
using Items;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UI
{
    public class ItemDragHandler : MonoBehaviour, IDragHandler, IDropHandler
    {
        private BoardCell _closestCell;

        public void OnDrag(PointerEventData eventData)
        {
            // Don't move inventory item if quantity is empty
            if (GetComponentInParent<InventoryItem>().ItemQuantity <= 0)
                return;
            
            transform.position = Input.mousePosition;

            // Highlight the nearest cell
            HighlightNearestCell();
        }

        public void OnDrop(PointerEventData eventData)
        {
            transform.localPosition = Vector3.zero;

            if (_closestCell) _closestCell.ResetCellColor();
            _closestCell = null;
        }

        private void HighlightNearestCell()
        {
            if (_closestCell) _closestCell.ResetCellColor();

            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            var hitColliders = Physics.RaycastAll(ray);

            _closestCell =
                (from hit in hitColliders
                    where hit.collider.gameObject.CompareTag("Grid Cell")
                    select hit.collider.gameObject.GetComponent<BoardCell>()).FirstOrDefault();

            if (_closestCell == null)
            {
                return;
            }

            var dropPosition = _closestCell.GetPosition();

            if (BoardManager.Instance.IsOccupied(dropPosition))
            {
                return;
            }

            _closestCell.HighlightCell();
        }
    }
}