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

        /// <summary>
        /// Add an item to the board.
        /// </summary>
        /// <param name="item"></param>
        public void AddItem(GameObject item)
        {
            _currentItem = item;
            item.transform.position = transform.position + new Vector3(0f, 0.5f, 0f);
        }

        /// <summary>
        /// Removed an item from the board.
        /// </summary>
        public void RemoveItem()
        {
            _currentItem = null;
        }

        /// <summary>
        /// Highlight a cell.
        /// </summary>
        public void HighlightCell()
        {
            _renderer.material = BoardManager.Instance.CellHighlightMaterial;
        }

        /// <summary>
        /// Select a cell.
        /// </summary>
        public void SelectCell()
        {
            _renderer.material = BoardManager.Instance.CellSelectMaterial;
        }

        /// <summary>
        /// Reset the cell color.
        /// </summary>
        public void ResetCellColor()
        {
            _renderer.material = BoardManager.Instance.CellDefaultMaterial;
        }

        /// <summary>
        /// Test if a cell has an item on it.
        /// </summary>
        /// <returns></returns>
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