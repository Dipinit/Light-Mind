using System;
using UnityEngine;

namespace Assets.Scripts.Map
{
    [Serializable]
    public class GridManager
    {
        private Vector2 _gridSize;
        private Vector2 _cellsOffSet;
        private Cell[] _cells;

        public GridManager SetGridSize(Vector2 size)
        {
            _gridSize = size;
            return this;
        }

        public GridManager SetCellsOffSet(Vector2 cellsOffSet)
        {
            _cellsOffSet = cellsOffSet;
            return this;
        }

        public void AddCell(Cell cell)
        {
            if (_cells == null)
            {
                _cells = new Cell [Mathf.RoundToInt(_gridSize.x * _gridSize.y)];
            }
            else
            {
                _cells.SetValue(cell, _cells.Length);
            }
        }

        public void SetCells(Cell[] cells)
        {
            _cells = cells;
        }

        public void InitCells()
        {
            if (_gridSize.x * _gridSize.y != _cells.Length)
            {
                throw new Exception("Grid Size != Cells");
            }

            // Creates a new object for every Cell and adds a sprite renderer
            foreach (var currCell in _cells)
            {
                var cellObject = new GameObject();
                cellObject.AddComponent<SpriteRenderer>().sprite = currCell.Sprite;
                cellObject.transform.position = new Vector3(currCell.Coords.x, currCell.Coords.y, 0);
            }
        }
    }
}