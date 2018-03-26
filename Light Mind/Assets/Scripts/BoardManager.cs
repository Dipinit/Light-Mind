using System;
using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour
{
    [Header("Board")]
    public GameObject Board;
    public Vector2Int BoardSize = new Vector2Int(8, 8);
        
    [Space(5)]
        
    [SerializeField] private GameObject _boardCellPrefab;
	public GameObject spawnerPrefab;
	public GameObject enderPrefab;

    [Header("Cell colors")]
    public Color CellDefaultColor;
    public Color CellHighlightColor = Color.yellow;

    [Space(5)]
    public List<Vector3> OccupiedPositions;

    private Vector2Int _lastGridSize;

    private void Start()
    {
        CreateBoard();
        AdjustCamera();
    }

    // Update is called once per frame
    private void Update()
    {
        if ((BoardSize.x == _lastGridSize.x) && (BoardSize.y == _lastGridSize.y)) return;


        _lastGridSize = BoardSize;
        UpdateBoard();
    }

    public void CreateBoard()
    {
        if (!Board)
        {
            Debug.Log("Could not find Board game object, creating one.");
            
            Board = new GameObject();
            Board.name = "Board";
        }

        for (var x = 0; x < BoardSize.x; x++)
        {
            for (var y = 0; y < BoardSize.y; y++)
            {
                var cell = Instantiate(_boardCellPrefab, Board.transform);
                cell.transform.position = new Vector3(x, y, 0);
                cell.GetComponent<SpriteRenderer>().color = CellDefaultColor;
            }
        }
    }

	public void AddTDElements() {
		// Add Spawner top left and Ender bottom right
		var spawner = Instantiate(spawnerPrefab, Board.transform);
		spawner.transform.position = new Vector3(0, 0, 0);

		var ender = Instantiate(enderPrefab, Board.transform);
		ender.transform.position = new Vector3(BoardSize.x - 1, BoardSize.y - 1, 0);
	}

    public void AddItemPosition(Vector3 pos)
    {
        if (!OccupiedPositions.Contains(pos))
        {
            OccupiedPositions.Add(pos);
            // Debug.Log(string.Format("Added position to grid: {0}.", pos));
        }
    }

    public void RemoveItemPosition(Vector3 pos)
    {
        if (OccupiedPositions.Contains(pos))
        {
            OccupiedPositions.Remove(pos);
            // Debug.Log(string.Format("Removed position from grid: {0}.", pos));
        }
    }

    public bool IsOccupied(Vector3 targetPosition)
    {
        if (!OccupiedPositions.Contains(targetPosition)) return false;

        Debug.Log(string.Format("Position {0} is occupied", targetPosition));
        return true;
    }

    private void UpdateBoard()
    {
        foreach (Transform child in Board.transform)
        {
            Destroy(child.gameObject);
        }

        CreateBoard();
        AdjustCamera();
    }

    private void AdjustCamera()
    {
        Camera.main.orthographicSize = Math.Max(BoardSize.x, BoardSize.y);
        Camera.main.transform.position = new Vector3((float) BoardSize.x / 2 - 0.5f, (float) BoardSize.y / 2 - 0.5f, -10.0f);
    }

    public void ResetCells()
    {
        ResetTilesColor();
    }

    private void ResetTilesColor()
    {
        foreach (Transform cell in Board.transform)
        {
            var cellSprite = cell.gameObject.GetComponent<SpriteRenderer>();
            cellSprite.color = CellDefaultColor;
        }
    }
}