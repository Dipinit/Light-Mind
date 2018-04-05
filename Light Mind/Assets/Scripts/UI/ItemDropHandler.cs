using System.Linq;
using Items;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UI
{
    public class ItemDropHandler : MonoBehaviour, IDropHandler
    {
        public void OnDrop(PointerEventData eventData)
        {
            var inventoryPanel = transform as RectTransform;

            if (RectTransformUtility.RectangleContainsScreenPoint(inventoryPanel, Input.mousePosition)) return;

            // Trying to drop item on board cell
            Debug.Log("Trying to drop an item from inventory...");
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            var hitColliders = Physics.RaycastAll(ray);

            var boardCell =
                (from hit in hitColliders
                    where hit.collider.gameObject.CompareTag("Grid Cell")
                    select hit.collider.gameObject.GetComponent<BoardCell>()).FirstOrDefault();

            if (boardCell == null)
            {
                Debug.Log("Did not found any cell to drop on.");
                return;
            }

            Debug.Log("Found board cell to drop on!");

            var itemPrefab = GetComponentInParent<InventoryItem>().ItemPrefab;
            var droppedItem = Instantiate(itemPrefab, GameManager.Instance.ItemsContainer.transform);
            var dropPosition = boardCell.GetPosition();

            if (!GameManager.Instance.BoardManager.AddItem(droppedItem, dropPosition))
            {
                Debug.Log("Could not drop item on this cell, replacing in inventory.");
                Destroy(droppedItem);

                // TODO: Replace item in inventory

                return;
            }

            Debug.Log(string.Format("Dropped {0} on board at position {1}", droppedItem, dropPosition));
        }
    }
}