using System;
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
            _renderer.material = BoardManager.Instance.CellHighlightMaterial;
        }

        public void SelectCell()
        {
            _renderer.material = BoardManager.Instance.CellSelectMaterial;
        }

        public void ResetCellColor()
        {
            _renderer.material = BoardManager.Instance.CellDefaultMaterial;
        }

        public bool IsOccupied()
        {
            return _currentItem != null;
        }

        public Vector2Int GetPosition()
        {
            return new Vector2Int(BoardManager.Instance.WorldToCellPosition(transform.position.x),
                BoardManager.Instance.WorldToCellPosition(transform.position.z));
        }

        public override String ToString()
        {
            return String.Format("Board Cell - X:{0} Y:{1}", GetPosition().x, GetPosition().y);
        }
    }
}