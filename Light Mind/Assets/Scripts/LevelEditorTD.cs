using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection.Emit;
using Assets.Scripts.Utilities;
using Items;
using Models;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;

public class LevelEditorTD : MonoBehaviour, IPointerClickHandler
{
	public static LevelEditorTD Instance;
	
	[Header("Manager Scripts")]
	public BoardManager BoardManager;
	
	[Header("Containers")] public GameObject ItemsContainer;

	[Header("Items")] 
	public GameObject MirrorPrefab;
	public GameObject FilterPrefab;
	public GameObject ObjectivePrefab;
	public GameObject ObstaclePrefab;
	public GameObject FilterMirrorPrefab;
	public GameObject LightSourcePrefab;
	public GameObject PrismPrefab;
	
	[Header("Inventory Items")] 
	public GameObject MirrorInventoryItemPrefab;
	public GameObject FilterMirrorInventoryItemPrefab;
	public GameObject PrismInventoryItemPrefab;
	public GameObject FilterInventoryItemPrefab;
	
	[Header("UI")]
	public GameObject Inventory;
	public GameObject ColorPicker;
	
	[Header("Turrets")]
	public GameObject StandardTurretPrefab;
	public GameObject MissileTurretPrefab;
	public GameObject LaserTurretPrefab;
	
	[Header("Turrets Inventory Items")] 
	public GameObject StandardTurretInventoryItemPrefab;
	public GameObject MissileTurretInventoryItemPrefab;
	public GameObject LaserTurretInventoryItemPrefab;

	[Header("Enemy Path")] 
	public GameObject SpawnerPrefab;
	public GameObject EnderPrefab;
	public GameObject PathPrefab;

	[Header("Enemy Navigation")] 
	public NavMeshSurface NavigationSurface;
	
	[Header("UI Steps")] 
	public GameObject Step10;
	public GameObject Step20;
	public GameObject Step21;
	public GameObject Step220;
	public GameObject Step221;
	public GameObject Step30;
	public GameObject Step40;
	public GameObject Step50;
	public GameObject Step60;

	[Header("Debug")] 
	public int _currentLevelHeight;
	public int _currentLevelWidth;
	public BoardPath _currentEnemyPath;
	public List<BoardPath> _enemyPaths;
	public Vector2Int _spawnPoint;
	public Vector2Int _endPoint;
	public string _currentWave;
	public List<string> _waves;
	public int _currentStep;
	public BoardCell _selectedCell;
	
	private JSONObject _levelData;
	
	private void Awake()
	{
		if (Instance == null)
			Instance = this;
		else if (Instance != this)
			Destroy(gameObject);
	}
	
	// Use this for initialization
	public void Start ()
	{		
		// Level data
		_levelData = new JSONObject();
		_levelData.AddField("Name", "");
		
		// Board
		_levelData.AddField("Board", new JSONObject());
		_levelData["Board"].AddField("Size", new JSONObject());
		_levelData["Board"]["Size"].AddField("X", 0);
		_levelData["Board"]["Size"].AddField("Y", 0);
		_levelData["Board"].AddField("CellSize", 4);
		_levelData["Board"].AddField("CellOffset", 1);
		_levelData["Board"].AddField("SpawnPoint", new JSONObject());
		_levelData["Board"]["SpawnPoint"].AddField("X", 0);
		_levelData["Board"]["SpawnPoint"].AddField("Y", 0);
		_levelData["Board"].AddField("Paths", new JSONObject(JSONObject.Type.ARRAY));
		_levelData["Board"].AddField("EndPoint", new JSONObject());
		_levelData["Board"]["EndPoint"].AddField("X", 0);
		_levelData["Board"]["EndPoint"].AddField("Y", 0);
		
		// Inventory
		_levelData.AddField("Inventory", new JSONObject());
		_levelData["Inventory"].AddField("Mirrors", 0);
		_levelData["Inventory"].AddField("MirrorFilters", 0);
		_levelData["Inventory"].AddField("Prisms", 0);
		_levelData["Inventory"].AddField("Filters", 0);
		_levelData["Inventory"].AddField("StandardTurret", 0);
		_levelData["Inventory"].AddField("MissileTurret", 0);
		_levelData["Inventory"].AddField("LaserTurret", 0);
		
		// Entities
		_levelData.AddField("Entities", new JSONObject(JSONObject.Type.ARRAY));
		
		// Waves
		_levelData.AddField("Waves", new JSONObject(JSONObject.Type.ARRAY));
		
		// Info
		_levelData.AddField("Info", new JSONObject());
		_levelData["Info"].AddField("Lives", 10);
		_levelData["Info"].AddField("SpawnInterval", 5);
		
		// Private variables initialization
		_currentStep = 10;
		Step10.SetActive(true);
		_enemyPaths = new List<BoardPath>();
		_currentEnemyPath = new BoardPath(0, 0, 0, 0);
		_currentWave = "";
		_waves = new List<string>();
	}
	
	/* Step 10 */

	public void ValidateStep10()
	{
		_levelData["Board"]["Size"]["X"].i = _currentLevelWidth;
		_levelData["Board"]["Size"]["Y"].i = _currentLevelHeight;
		_levelData["Board"]["CellSize"].i = 4;
		_levelData["Board"]["CellOffset"].i = 1;
		
		BoardManager.BoardSize.x = _currentLevelWidth;
		BoardManager.BoardSize.y = _currentLevelHeight;

		BoardManager.CellSize = (int) _levelData["Board"]["CellSize"].i;
		BoardManager.CellOffset = (int) _levelData["Board"]["CellOffset"].i;

		BoardManager.CreateBoardCells();

		Step10.SetActive(false);

		BoardManager.AdjustCamera();
		
		Step20.SetActive(true);
		_currentStep = 20;
	}

	public void UpdateBoardHeight(string input)
	{
		bool result = int.TryParse(input, out _currentLevelHeight);
	}
	
	public void UpdateBoardWidth(string input)
	{
		bool result = int.TryParse(input, out _currentLevelWidth);
	}
	
	/* Step 20 */

	public void ValideStep20()
	{
		if (_selectedCell == null) return;
		
		_levelData["Board"]["SpawnPoint"]["X"].i = _selectedCell.GetPosition().x;
		_levelData["Board"]["SpawnPoint"]["Y"].i = _selectedCell.GetPosition().y;
		
		BoardManager.SpawnPoint = _selectedCell.GetPosition();
		ResetSelectedCell();
		
		BoardManager.CreateBoardSpawner();
		
		Step20.SetActive(false);		
		Step21.SetActive(true);
		
		
		_currentStep = 21;
	}

	/* Step 21 */

	public void ValideStep21()
	{
		_levelData["Board"]["EndPoint"]["X"].i = _selectedCell.GetPosition().x;
		_levelData["Board"]["EndPoint"]["Y"].i = _selectedCell.GetPosition().y;
		
		BoardManager.EndPoint = _selectedCell.GetPosition();
		ResetSelectedCell();
		
		BoardManager.CreateBoardEnder();
		
		Step21.SetActive(false);		
		Step220.SetActive(true);
		_currentStep = 220;
	}
	
	/* Step 22 */

	public void ValideStep22()
	{
		foreach (var enemyPath in _enemyPaths)
		{
			JSONObject jsonPath = new JSONObject();
			jsonPath.AddField("X1", enemyPath.Start.x);
			jsonPath.AddField("Y1", enemyPath.Start.y);
			jsonPath.AddField("X2", enemyPath.End.x);
			jsonPath.AddField("Y2" ,enemyPath.End.y);

			_levelData["Board"]["Paths"].Add(jsonPath);
		}
		
		Step220.SetActive(false);
		Step221.SetActive(false);
		Step30.SetActive(true);
		_currentStep = 30;
	}

	public void ValidateStep220()
	{
		_currentEnemyPath.Start = _selectedCell.GetPosition();
		ResetSelectedCell();
		
		Debug.Log("Validating Step 220");
		Step220.SetActive(false);
		Step221.SetActive(true);
		_currentStep = 221;
	}
	
	public void ValidateStep221()
	{
		_currentEnemyPath.End = _selectedCell.GetPosition();
		ResetSelectedCell();
		
		Debug.Log("Validating Step 221");
		BoardPath clone = (BoardPath) _currentEnemyPath.Clone();
		
		BoardManager.Paths.Add(clone);
		BoardManager.CreateBoardEnemyPaths();
		
		_enemyPaths.Add(clone);

		foreach (BoardPath boardPath in BoardManager.Paths)
		{
			Debug.Log(boardPath);
		}
		
		Step220.SetActive(true);
		Step221.SetActive(false);
		_currentStep = 220;
	}
	
	/* Step 30 */

	public void ValidateStep30()
	{
		foreach (var wave in _waves)
		{
			JSONObject jsonWave = new JSONObject();
			jsonWave.AddField("Enemies", wave);
			_levelData["Waves"].Add(jsonWave);
		}
		
		//Step40.SetActive(true);
		Step30.SetActive(false);
		_currentStep = 40;
	}

	public void AddEnemyToWave(string enemyCode)
	{
		_currentWave = String.Concat(_currentWave, enemyCode);
	}

	public void AddWave()
	{
		_waves.Add((string) _currentWave.Clone());
		_currentWave = "";
	}

	public void OnPointerClick(PointerEventData eventData)
	{
		SelectCell(eventData);
	}

	private void SelectCell(PointerEventData eventData)
	{
		Debug.Log("click");
		Vector3 screenPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		screenPoint.y = 2.0f;
		
		Vector2Int cellPosition = new Vector2Int(
			BoardManager.WorldToCellPosition(screenPoint.x),
			BoardManager.WorldToCellPosition(screenPoint.z)
		);

		BoardCell boardCell = BoardManager.GetCellAt(cellPosition);
		if (boardCell != null)
		{
			if (_selectedCell != null) _selectedCell.ResetCellColor();
			_selectedCell = boardCell;
			_selectedCell.HighlightCell();
		}
		
		Debug.Log(_selectedCell);
	}

	private void ResetSelectedCell()
	{
		_selectedCell.ResetCellColor();
		_selectedCell = null;
		Debug.Log(_selectedCell);
	}

	private Vector2Int WorldVector3ToCellVector2(Vector3 worldVector)
	{
		Vector2Int cellVector = new Vector2Int(
			BoardManager.WorldToCellPosition(worldVector.x),
			BoardManager.WorldToCellPosition(worldVector.z)
		);

		return cellVector;
	}
}
