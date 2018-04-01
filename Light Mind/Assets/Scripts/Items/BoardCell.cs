using UnityEngine;

namespace Items
{
    [RequireComponent(typeof(Renderer))]
    public class BoardCell : MonoBehaviour
    {
        private Renderer _renderer;
        private GameObject _currentItem;

        private void Start()
        {
            _renderer = GetComponent<Renderer>();
        }

        public void AddItem(GameObject item)
        {
            _currentItem = item;
            item.transform.position = transform.position + new Vector3(0f, 0.5f, 0f);
        }

        public void RemoveItem()
        {
            _currentItem = null;
        }

        public void HighlightCell()
        {
            _renderer.material = GameManager.Instance.BoardManager.CellHighlightMaterial;
        }

        public void ResetCellColor()
        {
            _renderer.material = GameManager.Instance.BoardManager.CellDefaultMaterial;
        }

        public bool IsOccupied()
        {
            return _currentItem != null;
        }
    }
}