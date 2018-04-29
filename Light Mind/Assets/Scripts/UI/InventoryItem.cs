using System;
using Behaviors;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Image = UnityEngine.UI.Image;

namespace UI
{
    public class InventoryItem : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IBeginDragHandler,
        IDragHandler, IEndDragHandler
    {
        private Image _image;
        private Text _text;

        private GameObject _instanciatedItem;

        public Sprite ItemImage;
        public Animator Animator;

        public GameObject ItemPrefab;
        public int ItemQuantity;
        public String ItemCode;

        // Use this for initialization
        private void Start()
        {
            _image = transform.Find("Image").gameObject.GetComponent<Image>();
            _text = transform.Find("Text").gameObject.GetComponent<Text>();

            _image.sprite = ItemImage;
        }

        // Update is called once per frame
        private void Update()
        {
            UpdateItemQuantityText();
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            // TODO: Highlighter l'Inventory Item au survol du pointeur
           
            // throw new System.NotImplementedException();
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            // TODO: Ne plus highlighter l'Inventory Item à la sortie du pointeur
            
            // throw new System.NotImplementedException();
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (ItemQuantity <= 0) return;
            
            GameObject itemsContainer =
                BoardManager.Instance.EditorMode
                    ? LevelEditorTD.Instance.ItemsContainer
                    : GameManager.Instance.ItemsContainer;
            
            ItemQuantity--;
            _instanciatedItem = Instantiate(ItemPrefab, Input.mousePosition, Quaternion.identity, itemsContainer.transform);
            RaySensitive raySensitive = _instanciatedItem.GetComponent<RaySensitive>();
            if (raySensitive && raySensitive.MeshCollider) raySensitive.MeshCollider.convex = false;

        }

        public void OnDrag(PointerEventData eventData)
        {
            if (_instanciatedItem == null) return;
            
            _instanciatedItem.GetComponent<DragAndDrop>().UpdateDraggedPosition();
            _instanciatedItem.GetComponent<DragAndDrop>().HighlightNearestCell();
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (_instanciatedItem == null) return;

            var inventoryPanel = GameObject.Find("Inventory").GetComponent<RectTransform>();

            if (inventoryPanel.rect.Contains(Input.mousePosition))
            {
                Debug.Log("Item not dragged out of panel, aborting.");

                // TODO: Put an return animation to the start position of the object.
                ItemQuantity++;
                Destroy(_instanciatedItem);
            }
            else
            {
                _instanciatedItem.GetComponent<DragAndDrop>().DropItem();
            }

            _instanciatedItem = null;
        }

        private void UpdateItemQuantityText()
        {
            _text.text = string.Format("x {0}", ItemQuantity);
        }
    }
}