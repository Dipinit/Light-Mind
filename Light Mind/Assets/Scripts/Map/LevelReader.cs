using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System.Text;
using System;
using System.IO;

public class LevelReader : MonoBehaviour
{
	public GridManager grid = new GridManager ();


	void Start ()
	{
		// Create a cell
		Cell cell = new Cell (SpriteUtility.LoadSprite ("../Light Mind/Assets/Textures/Tiles/blue.png", 64, 64), new Vector2 (0, 0));

		// Object to Json
		string json = JsonUtility.ToJson (cell);
		Debug.Log (json);

		// Json to object
		Cell cellCopy = JsonUtility.FromJson<Cell> (json);


		// Create array of cells
		Cell cell2 = new Cell (SpriteUtility.LoadSprite ("../Light Mind/Assets/Textures/Tiles/blue.png", 64, 64), new Vector2 (0, 2));
		Cell[] cells = new Cell [2];
		cells [0] = cell;
		cells [1] = cell2;

		// Object[] to Json
		string jsonArray = JsonArrayUtility.arrayToJson<Cell> (cells);
		Debug.Log (jsonArray);

		// Json to object
		Cell[] cellsCopy = JsonArrayUtility.getJsonArray<Cell> (jsonArray);
		Debug.Log (cellsCopy.Length);

		// Create a Grid Exemple
		List<Cell> cellList = new List<Cell> ();
		for (int i = 0; i < 6; i++) {
			for (int j = 0; j < 6; j++) {
				Cell tmpCell = new Cell (SpriteUtility.LoadSprite ("../Light Mind/Assets/Textures/Tiles/blue.png", 64, 64), new Vector2 (i, j));
				cellList.Add (tmpCell);
			}
		}

		// Jsonify test
		string jsonList = JsonArrayUtility.arrayToJson<Cell> (cellList.ToArray ());
		Debug.Log (jsonList);

		// Unjsonify test
		cellsCopy = JsonArrayUtility.getJsonArray<Cell> (jsonList);
		Debug.Log (cellsCopy.Length);

		// Initialise real grid with unjsonified grid
		grid.setGridSize (new Vector2 (6, 6)).setCellsOffSet (new Vector2 (1, 1)).setCells (cellsCopy);
		grid.initCells ();
	}

	void Update ()
	{
		
	}

}
