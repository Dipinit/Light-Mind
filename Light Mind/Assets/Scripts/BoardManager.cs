using System;
using System.Collections.Generic;
using Models;
using UnityEngine;

public class BoardManager : MonoBehaviour
{
    [Header("Board")] public GameObject Board;
    public Vector2Int BoardSize = new Vector2Int(8, 8);

    [Space(5)] [SerializeField] private GameObject _boardCellPrefab;

    [Header("Cells")] public int CellSize = 4;
    public int CellOffset = 1;
    public Material CellDefaultMaterial;
    public Material CellHighlightMaterial;

    [Header("Path")] public Vector2Int SpawnPoint;
    public List<BoardPath> Paths = new List<BoardPath>();
    public Vector2Int EndPoint;

    [Space(5)] public List<Vector3> OccupiedPositions;

    private Vector2Int _lastGridSize;
    private int _lastCellOffset;

    private void Start()
    {
        CreateBoard();
        AdjustCamera();
    }

    // Update is called once per frame
    private void Update()
    {
        if (BoardSize.x == _lastGridSize.x && BoardSize.y == _lastGridSize.y && _lastCellOffset == CellOffset) return;

        _lastGridSize = BoardSize;
        _lastCellOffset = CellOffset;
        UpdateBoard();
    }

    public void CreateBoard()
    {
        if (!Board)
        {
            Debug.Log("Could not find Board game object, creating one.");

            Board = new GameObject {name = "Board"};
        }

        // Create board cells
        for (var x = 0; x < BoardSize.x; x++)
        {
            var xPos = x == 0 ? 0 : CellSize * x + CellOffset * x;
            for (var y = 0; y < BoardSize.y; y++)
            {
                var yPos = y == 0 ? 0 : CellSize * y + CellOffset * y;
                var cell = Instantiate(_boardCellPrefab, Board.transform);
                cell.transform.position = new Vector3(xPos, 1, yPos);
                cell.GetComponent<Renderer>().material = CellDefaultMaterial;
            }
        }

        // Create spawner
        var spawner = Instantiate(GameManager.Instance.SpawnerPrefab, Board.transform);
        spawner.transform.position =
            new Vector3(CellToWorldPosition(SpawnPoint.x), 1.5f + spawner.transform.localScale.y / 2,
                CellToWorldPosition(SpawnPoint.y));

        // Create enemy walkable paths
        foreach (var pathData in Paths)
        {
            // Delete board cells above path
            for (var i = pathData.Start.x; i <= pathData.End.x; i++)
            {
                for (var j = pathData.Start.y; j <= pathData.End.y; j++)
                {
                    DeleteCellAtPosition(i, j);
                }
            }

            // Instantiate path
            var path = Instantiate(GameManager.Instance.PathPrefab, Board.transform);
            path.transform.localScale = pathData.GetPathScale(CellSize, CellOffset);
            path.transform.position = pathData.GetPathPosition(CellSize, CellOffset);
        }

        // Create ender
        var ender = Instantiate(GameManager.Instance.EnderPrefab, Board.transform);
        ender.transform.position =
            new Vector3(CellToWorldPosition(EndPoint.x), 1.5f + ender.transform.localScale.y / 2,
                CellToWorldPosition(EndPoint.y));
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
        Camera.main.orthographicSize = CellToWorldPosition(Math.Max(BoardSize.x, BoardSize.y));
        Camera.main.transform.position =
            new Vector3(CellToWorldPosition(BoardSize.x - 1) / 2.0f, 10.0f,
                CellToWorldPosition(BoardSize.y - 1) / 2.0f);
    }

    private void DeleteCellAtPosition(int x, int y)
    {
        foreach (Transform cell in Board.transform)
        {
            if (!cell.CompareTag("Grid Cell")) continue;

            var targetPosition = new Vector3(CellToWorldPosition(x), 1.0f, CellToWorldPosition(y));
            
            Debug.Log(string.Format("Target position:{0}, Cell position: {1}", targetPosition, cell.position));
            if (cell.position.Equals(targetPosition))
            {
                Destroy(cell.gameObject);
            }
        }
    }

    public void ResetCells()
    {
        foreach (Transform cell in Board.transform)
        {
            var cellSprite = cell.gameObject.GetComponent<Renderer>();
            cellSprite.material = CellDefaultMaterial;
        }
    }

    public float CellToWorldPosition(int coordinate)
    {
        return coordinate == 0 ? 0 : coordinate * (CellSize + CellOffset);
    }
}