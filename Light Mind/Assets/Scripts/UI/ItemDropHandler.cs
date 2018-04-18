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

            // Check if item is not still in inventory canvas
            if (RectTransformUtility.RectangleContainsScreenPoint(inventoryPanel, Input.mousePosition)) return;

            // Check item quantity
            if (GetComponentInParent<InventoryItem>().ItemQuantity <= 0)
            {
                Debug.Log("Not enough quantity to drop item.");
                return;
            }

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

            var itemsContainer =
                BoardManager.Instance.EditorMode
                    ? LevelEditorTD.Instance.ItemsContainer
                    : GameManager.Instance.ItemsContainer;

            var itemPrefab = GetComponentInParent<InventoryItem>().ItemPrefab;
            var droppedItem = Instantiate(itemPrefab, itemsContainer.transform);
            var dropPosition = boardCell.GetPosition();

            if (!BoardManager.Instance.AddItem(droppedItem, dropPosition))
            {
                Debug.Log("Could not drop item on this cell, replacing in inventory.");
                Destroy(droppedItem);

                return;
            }

            // Decrease item quantity
            GetComponentInParent<InventoryItem>().ItemQuantity--;

            Debug.Log(string.Format("Dropped {0} on board at position {1}", droppedItem, dropPosition));
        }
    }
}