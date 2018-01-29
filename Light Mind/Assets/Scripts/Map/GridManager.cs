using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class GridManager
{

	public GridManager ()
	{
	}

	public GridManager setGridSize (Vector2 gridSize)
	{
		this.gridSize = gridSize;
		return this;
	}

	public GridManager setCellsOffSet (Vector2 cellsOffSet)
	{
		this.cellsOffSet = cellsOffSet;
		return this;
	}

	public void addCell (Cell cell)
	{
		if (this.cells == null) {
			this.cells = new Cell [Mathf.RoundToInt (gridSize.x * gridSize.y)];
		} else {
			cells.SetValue (cell, cells.Length);
		}
	}

	public void setCells (Cell[] cells)
	{
		this.cells = cells;
	}

	private Vector2 gridSize;
	private Vector2 cellsOffSet;
	private Cell[] cells;

	public void initCells ()
	{
		if (gridSize.x * gridSize.y != cells.Length) {
			//throw new Exception ("Grid Size != Cells");
		}
		// Creates a new object for every Cell and adds a sprite renderer
		for (int i = 0; i < cells.Length; i++) {
			Cell currCell = cells [i];
			GameObject cellObject = new GameObject ();
			cellObject.AddComponent<SpriteRenderer> ().sprite = currCell.sprite;
			cellObject.transform.position = new Vector3 (currCell.coords.x, currCell.coords.y, 0);
		}
	}
}
