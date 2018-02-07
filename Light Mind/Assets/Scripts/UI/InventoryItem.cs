using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Image = UnityEngine.UI.Image;

namespace Assets.Scripts.UI
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
            
            throw new System.NotImplementedException();
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            // TODO: Ne plus highlighter l'Inventory Item à la sortie du pointeur
            
            throw new System.NotImplementedException();
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (ItemQuantity <= 0) return;
            ItemQuantity--;
            _instanciatedItem = Instantiate(ItemPrefab, Input.mousePosition, Quaternion.identity);
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (_instanciatedItem == null) return;
            var pointerWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            pointerWorldPosition.z = 0;
            _instanciatedItem.transform.position = pointerWorldPosition;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (_instanciatedItem == null) return;

            var inventoryPanel = GameObject.Find("Inventory").GetComponent<RectTransform>();

            if (inventoryPanel.rect.Contains(Input.mousePosition))
            {
                Debug.Log("Item not dragged out of panel, aborting.");

                // TODO: Mettre une animation de retour vers la position de départ de l'objet

                ItemQuantity++;
                Destroy(_instanciatedItem);
            }
            else
                _instanciatedItem.transform.Find("Quad").GetComponent<DragAndDrop>().OnMouseUp();

            _instanciatedItem = null;
        }

        private void UpdateItemQuantityText()
        {
            _text.text = string.Format("x {0}", ItemQuantity);
        }
    }
}