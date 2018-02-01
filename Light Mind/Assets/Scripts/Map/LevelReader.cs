using System.Collections.Generic;
using Assets.Scripts.Utilities;
using UnityEngine;

namespace Assets.Scripts.Map
{
    public class LevelReader : MonoBehaviour
    {
        public GridManager Grid = new GridManager();


        private void Start()
        {
            // Create a cell
            var cell = new Cell(SpriteUtility.LoadSprite("../Light Mind/Assets/Textures/Tiles/blue.png", 64, 64),
                new Vector2(0, 0));

            // Object to Json
            var json = JsonUtility.ToJson(cell);
            Debug.Log(json);

            // Json to object
            var cellCopy = JsonUtility.FromJson<Cell>(json);
            Debug.Log(cellCopy);


            // Create array of cells
            var cell2 = new Cell(SpriteUtility.LoadSprite("../Light Mind/Assets/Textures/Tiles/blue.png", 64, 64),
                new Vector2(0, 2));
            var cells = new Cell [2];
            cells[0] = cell;
            cells[1] = cell2;

            // Object[] to Json
            var jsonArray = JsonArrayUtility.ArrayToJson(cells);
            Debug.Log(jsonArray);

            // Json to object
            var cellsCopy = JsonArrayUtility.GetJsonArray<Cell>(jsonArray);
            Debug.Log(cellsCopy.Length);

            // Create a Grid Exemple
            var cellList = new List<Cell>();
            for (var i = 0; i < 6; i++)
            {
                for (var j = 0; j < 6; j++)
                {
                    var tmpCell =
                        new Cell(SpriteUtility.LoadSprite("../Light Mind/Assets/Textures/Tiles/blue.png", 64, 64),
                            new Vector2(i, j));
                    cellList.Add(tmpCell);
                }
            }

            // Jsonify test
            var jsonList = JsonArrayUtility.ArrayToJson(cellList.ToArray());
            Debug.Log(jsonList);

            // Unjsonify test
            cellsCopy = JsonArrayUtility.GetJsonArray<Cell>(jsonList);
            Debug.Log(cellsCopy.Length);

            // Initialise real grid with unjsonified grid
            Grid.SetGridSize(new Vector2(6, 6)).SetCellsOffSet(new Vector2(1, 1)).SetCells(cellsCopy);
            Grid.InitCells();
        }
    }
}