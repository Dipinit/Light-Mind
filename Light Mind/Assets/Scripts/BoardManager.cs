using System;
using System.Collections.Generic;
using Items;
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

    private Vector2Int _lastBoardSize;
    private int _lastCellOffset;

    [Header("Mode")]
    public bool EditorMode = false;

    private void Start()
    {
        _lastBoardSize = BoardSize;
        _lastCellOffset = CellOffset;
    }

    // Update is called once per frame
    private void Update()
    {
        if (EditorMode || (_lastBoardSize == BoardSize && _lastCellOffset == CellOffset)) return;

        _lastBoardSize = BoardSize;
        _lastCellOffset = CellOffset;
        UpdateBoard();
    }

    public void CreateBoard()
    {
        CreateBoardCells();
        CreateBoardSpawner();
        CreateBoardEnemyPaths();
        CreateBoardEnder();
        CreateBoardEnemyNavigation();
        AdjustCamera();
    }

    public void CreateBoardCells()
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
    }

    public void CreateBoardSpawner()
    {
        // Create spawner
        GameObject prefab = EditorMode ? LevelEditorTD.Instance.SpawnerPrefab : GameManager.Instance.SpawnerPrefab;
        var spawner = Instantiate(prefab, Board.transform);
        spawner.transform.position =
            new Vector3(CellToWorldPosition(SpawnPoint.x), 1.5f + spawner.transform.localScale.y / 2,
                CellToWorldPosition(SpawnPoint.y));
    }

    public void CreateBoardEnder()
    {
        // Create ender
        GameObject prefab = EditorMode ? LevelEditorTD.Instance.EnderPrefab : GameManager.Instance.EnderPrefab;
        var ender = Instantiate(prefab, Board.transform);
        ender.transform.position =
            new Vector3(CellToWorldPosition(EndPoint.x), 1.5f + ender.transform.localScale.y / 2,
                CellToWorldPosition(EndPoint.y));
    }

    public void CreateBoardEnemyPaths()
    {
        // Create enemy walkable paths
        GameObject prefab = EditorMode ? LevelEditorTD.Instance.PathPrefab : GameManager.Instance.PathPrefab;
        foreach (var pathData in Paths)
        {
            // Delete board cells above path
            for (var i = pathData.Start.x; i <= pathData.End.x; i++)
            {
                for (var j = pathData.Start.y; j <= pathData.End.y; j++)
                {
                    DeleteCellAt(new Vector2Int(i, j));
                }
            }

            // Instantiate path
            var path = Instantiate(prefab, Board.transform);
            path.transform.localScale = pathData.GetPathScale(CellSize, CellOffset);
            path.transform.position = pathData.GetPathPosition(CellSize, CellOffset);
        }
    }

    public void CreateBoardEnemyNavigation()
    {
        // Build navigation using NavMesh
        Debug.Log(GameManager.Instance.NavigationSurface);
        GameManager.Instance.NavigationSurface.BuildNavMesh();
    }

    public bool AddItem(GameObject item, Vector2Int position)
    {
        try
        {
            var cell = GetCellAt(position);
            if (cell.IsOccupied()) return false;
            cell.AddItem(item);
            //Debug.Log(string.Format("Added item {0} to grid at position {1}.", item, position));
            return true;
        }
        catch (NullReferenceException e)
        {
            Debug.LogException(e);
            return false;
        }
    }

    public bool RemoveItemAt(Vector2Int position)
    {
        try
        {
            var cell = GetCellAt(position);
            if (!cell.IsOccupied()) return false;
            cell.RemoveItem();
            //Debug.Log(string.Format("Removed item from grid at position {0}.", position));
            return true;
        }
        catch (NullReferenceException e)
        {
            Debug.LogException(e);
            return false;
        }
    }

    public bool IsOccupied(Vector2Int position)
    {
        try
        {
            var cell = GetCellAt(position);
            //Debug.Log(string.Format("Position {0} occupied: {1}", position, cell.IsOccupied()));
            return cell.IsOccupied();
        }
        catch (NullReferenceException e)
        {
            Debug.LogException(e);
            return false;
        }
    }

    private void UpdateBoard()
    {
        Debug.Log("UpdateBoard requested.");

        foreach (Transform child in Board.transform)
        {
            Destroy(child.gameObject);
        }

        CreateBoard();
    }

    public void AdjustCamera()
    {
        Camera.main.orthographicSize = CellToWorldPosition(Math.Max(BoardSize.x, BoardSize.y));
        Camera.main.transform.position =
            new Vector3(CellToWorldPosition(BoardSize.x - 1) / 2.0f, 10.0f,
                CellToWorldPosition(BoardSize.y - 1) / 2.0f);
    }

    public void ResetCells()
    {
        foreach (Transform cell in Board.transform)
        {
            if (!cell.CompareTag("Grid Cell")) continue;

            cell.GetComponent<BoardCell>().ResetCellColor();
        }
    }

    public BoardCell GetCellAt(Vector2Int position)
    {
        var targetPosition = new Vector3(CellToWorldPosition(position.x), 1.0f, CellToWorldPosition(position.y));
        foreach (Transform cell in Board.transform)
        {
            if (!cell.CompareTag("Grid Cell")) continue;

            

            if (cell.position.Equals(targetPosition))
            {
                return cell.gameObject.GetComponent<BoardCell>();
            }
        }

        return null;
    }

    private void DeleteCellAt(Vector2Int position)
    {
        var cell = GetCellAt(position);
        if (cell) Destroy(cell.gameObject);
    }

    public int WorldToCellPosition(float worldCoordinate)
    {
        
        
        var coordinate = Mathf.RoundToInt(worldCoordinate);
        coordinate = coordinate == 0 ? 0 : coordinate / (CellSize + CellOffset);
        
        return coordinate;
    }

    public float CellToWorldPosition(int coordinate)
    {
        return coordinate == 0 ? 0 : coordinate * (CellSize + CellOffset);
    }
}