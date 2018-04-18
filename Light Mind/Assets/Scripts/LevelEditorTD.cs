using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection.Emit;
using System.Runtime.Serialization.Formatters.Binary;
using Assets.Scripts.Utilities;
using Behaviors;
using Items;
using Models;
using UI;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

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
	public JSONObject _currentWave;
	public JSONObject _waves;
	public int _currentStep;
	public BoardCell _selectedCell;
	public int _currentFilterCount;
	public int _currentMirrorCount;
	public int _currentPrismCount;
	public int _currentFilterMirrorCount;
	public int _currentStandardTurretCount;
	public int _currentMissileTurretCount;
	public int _currentLaserTurretCount;
	public string _currentLevelName;
	public int _currentLives;
	public float _currentDefaultSpawnInterval;
	public int _currentDefaultHitpoints;
	public float _currentDefaultSpeed;
	public string _currentDefaultColor;
	
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
		_levelData.AddField("FunFact", "");
		
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
		_levelData["Info"].AddField("DefaultSpawnInterval", 5.0f);
		_levelData["Info"].AddField("DefaultHitpoints", 100);
		_levelData["Info"].AddField("DefaultSpeed", 10.0f);
		_levelData["Info"].AddField("DefaultColor", "white");
		_levelData["Info"].AddField("SpawnInterval", 5);
		
		// Private variables initialization
		_currentStep = 10;
		
		Step10.SetActive(true);
		_enemyPaths = new List<BoardPath>();
		_currentEnemyPath = new BoardPath(0, 0, 0, 0);
		_currentWave = new JSONObject();
		_currentWave.AddField("Enemies", new JSONObject(JSONObject.Type.ARRAY));
		_waves = new JSONObject(JSONObject.Type.ARRAY);
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
		
		Step60.SetActive(true);
		_currentStep = 60;
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
		BoardManager.CreateBoardEnemyPaths();
		
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
		colorSelectedCellOnPath(clone);
		//BoardManager.CreateBoardEnemyPaths();
		
		_enemyPaths.Add(clone);

		foreach (BoardPath boardPath in BoardManager.Paths)
		{
			Debug.Log(boardPath);
		}
		
		Step220.SetActive(true);
		Step221.SetActive(false);
		_currentStep = 220;
	}

	public void colorSelectedCellOnPath(BoardPath boardPath)
	{
		for (int x = boardPath.Start.x; x <= boardPath.End.x; x++)
		{
			for (int y = boardPath.Start.y; y <= boardPath.End.y; y++)
			{
				BoardCell cell = BoardManager.GetCellAt(new Vector2Int(x, y));
				Debug.Log(cell);
				if (cell != null)
				{
					Debug.Log("mdr");
					Debug.Log(cell);
					cell.SelectCell();
				}
			}
		}
	}
	
	/* Step 30 */

	public void ValidateStep30()
	{
		_levelData["Waves"] = _waves;
		
		Step40.SetActive(true);
		Step30.SetActive(false);

		PrepareStep40();
		
		_currentStep = 40;
	}

	public void AddEnemyToWave(string enemyCode)
	{
		JSONObject enemy = new JSONObject();
		enemy.AddField("Hitpoints", _currentDefaultHitpoints);
		enemy.AddField("Speed", _currentDefaultSpeed);
		enemy.AddField("Color", enemyCode);
		_currentWave["Enemies"].Add(enemy);
	}

	public void AddWave()
	{
		string waveString = _currentWave.ToString();
		Debug.Log(waveString);
		JSONObject waveCopy = new JSONObject(waveString);
		Debug.Log(waveCopy);
		_waves.Add(waveCopy);
		_currentWave = new JSONObject();
		_currentWave.AddField("Enemies", new JSONObject(JSONObject.Type.ARRAY));
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

	public void PrepareStep40()
	{
		LoadFullInventory();
		Inventory.SetActive(true);
	}
	
	public void ValidateStep40()
	{
		Step50.SetActive(true);
		Step40.SetActive(false);
		SaveItems();
		_currentStep = 50;
	}
	
	// Load player inventory
	private void LoadFullInventory()
	{
		CreateInventoryItem(MirrorInventoryItemPrefab, "mirror", 99);
		CreateInventoryItem(FilterMirrorInventoryItemPrefab, "mirror-filter", 99);
		CreateInventoryItem(PrismInventoryItemPrefab, "prism", 99);
		CreateInventoryItem(FilterInventoryItemPrefab, "filter", 99);
		CreateInventoryItem(StandardTurretInventoryItemPrefab, "standard-turret", 99);
		CreateInventoryItem(MissileTurretInventoryItemPrefab, "missile-turret", 99);
		CreateInventoryItem(LaserTurretInventoryItemPrefab, "laser-turret", 99);
	}
	
	private void CreateInventoryItem(GameObject itemPrefab, string itemCode, int count)
	{
		GameObject itemGameObject = Instantiate(itemPrefab, Inventory.transform);
		InventoryItem inventoryItem = itemGameObject.GetComponent<InventoryItem>();
		inventoryItem.ItemQuantity = count;
		inventoryItem.ItemCode = itemCode;
	}

	private void SaveItems()
	{
		foreach (Transform item in ItemsContainer.transform)
		{
			RaySensitive raySensitive = item.GetComponent<RaySensitive>();
			Debug.Log(raySensitive.ItemCode);
			SaveItem(item);
		}
		
		Debug.Log(_levelData.ToString());
	}

	private void SaveItem(Transform item)
	{
		JSONObject jsonObject = new JSONObject();

		jsonObject.AddField("X", BoardManager.WorldToCellPosition(item.position.x));
		jsonObject.AddField("Y", BoardManager.WorldToCellPosition(item.position.z));
		jsonObject.AddField("Draggable", false);
		
		RaySensitive raySensitive = item.GetComponent<RaySensitive>();
		Debug.Log(raySensitive.ItemCode);
		if (raySensitive.ItemCode == "mirror") SaveMirrorItem(item, jsonObject);
		else if (raySensitive.ItemCode == "filter") SaveFilterItem(item, jsonObject);
		else if (raySensitive.ItemCode == "prism") SavePrismItem(item, jsonObject);
		else if (raySensitive.ItemCode == "filter-mirror") SaveFilterMirrorItem(item, jsonObject);
		else if (raySensitive.ItemCode == "obstacle") SaveObstacleItem(item, jsonObject);
		else if (raySensitive.ItemCode == "standard-turret") SaveStandardTurretItem(item, jsonObject);
		else if (raySensitive.ItemCode == "missile-turret") SaveMissileTurretItem(item, jsonObject);
		else if (raySensitive.ItemCode == "laser-turret") SaveLaserTurretItem(item, jsonObject);
		else if (raySensitive.ItemCode == "light-source") SaveLightSourceItem(item, jsonObject);
		else
			return;
		_levelData["Entities"].Add(jsonObject);
	}

	private void SaveMirrorItem(Transform item, JSONObject jsonObject)
	{
		Mirror mirror = item.GetComponent<Mirror>();
		jsonObject.AddField("Type", "Mirror");
		jsonObject.AddField("Orientation", (int) mirror.Orientation);		
	}
	
	private void SaveFilterItem(Transform item, JSONObject jsonObject)
	{
		Filter filter = item.GetComponent<Filter>();
		jsonObject.AddField("Type", "Filter");
		SaveItemColor(jsonObject, filter.Color);
	}

	private void SavePrismItem(Transform item, JSONObject jsonObject)
	{
		Prism prism = item.GetComponent<Prism>();
		jsonObject.AddField("Type", "Prism");
		SaveItemColor(jsonObject, prism.Color);
	}
	private void SaveFilterMirrorItem(Transform item, JSONObject jsonObject)
	{
		FilterMirror filterMirror = item.GetComponent<FilterMirror>();
		jsonObject.AddField("Type", "Filter Mirror");
		SaveItemColor(jsonObject, filterMirror.Color);
		jsonObject.AddField("Orientation", (int) filterMirror.Orientation);
	}

	private void SaveObstacleItem(Transform item, JSONObject jsonObject)
	{
		jsonObject.AddField("Type", "Obstacle");
	}
	
	private void SaveStandardTurretItem(Transform item, JSONObject jsonObject)
	{
		jsonObject.AddField("Type", "Standard Turret");
	}
	
	private void SaveMissileTurretItem(Transform item, JSONObject jsonObject)
	{
		jsonObject.AddField("Type", "Missile Turret");
	}
	
	private void SaveLaserTurretItem(Transform item, JSONObject jsonObject)
	{
		jsonObject.AddField("Type", "Laser Turret");
	}

	private void SaveLightSourceItem(Transform item, JSONObject jsonObject)
	{
		
	}

	private void SaveItemColor(JSONObject jsonObject, RayColor color)
	{
		jsonObject.AddField("Red", color.R);
		jsonObject.AddField("Green", color.G);
		jsonObject.AddField("Blue", color.B);
	}
	
	public void ValidateStep50()
	{
		_levelData["Inventory"]["Mirrors"].i = _currentMirrorCount;
		_levelData["Inventory"]["Filters"].i = _currentFilterCount;
		_levelData["Inventory"]["Prisms"].i = _currentPrismCount;
		_levelData["Inventory"]["MirrorFilters"].i = _currentFilterMirrorCount;
		_levelData["Inventory"]["StandardTurret"].i = _currentStandardTurretCount;
		_levelData["Inventory"]["MissileTurret"].i = _currentMissileTurretCount;
		_levelData["Inventory"]["LaserTurret"].i = _currentLaserTurretCount;
		
		Step50.SetActive(false);	
		_currentStep = 70;
		ValidateStep70();
		
		Debug.Log(_levelData.ToString());
	}

	public void UpdateMirrorCount(string input)
	{
		bool result = int.TryParse(input, out _currentMirrorCount);
	}

	public void UpdateFilterCount(string input)
	{
		bool result = int.TryParse(input, out _currentFilterCount);
	}

	public void UpdatePrismCount(string input)
	{
		bool result = int.TryParse(input, out _currentPrismCount);
	}

	public void UpdateFilterMirrorCount(string input)
	{
		bool result = int.TryParse(input, out _currentFilterMirrorCount);
	}

	public void UpdateStandardTurretCount(string input)
	{
		bool result = int.TryParse(input, out _currentStandardTurretCount);
	}

	public void UpdateMissileTurretCount(string input)
	{
		bool result = int.TryParse(input, out _currentMissileTurretCount);
	}

	public void UpdateLaserTurretCount(string input)
	{
		bool result = int.TryParse(input, out _currentLaserTurretCount);
	}

	public void ValidateStep60()
	{
		_levelData["Name"].str = _currentLevelName;
		_levelData["FunFact"].str = "Savez-vous que lorsque vous créez un niveau via l'éditeur, vous ne pourrez plus jamais le modifier, ni le supprimer ?";
		_levelData["Info"]["Lives"].i = _currentLives;
		_levelData["Info"]["DefaultSpawnInterval"].n = _currentDefaultSpawnInterval;
		_levelData["Info"]["DefaultHitpoints"].i = _currentDefaultHitpoints;
		_levelData["Info"]["DefaultSpeed"].n = _currentDefaultSpeed;
		_levelData["Info"]["DefaultColor"].str = "White";
	
		Step60.SetActive(false);
		Step20.SetActive(true);

	}
	
	public void UpdateLevelName(string input)
	{
		_currentLevelName = input;
	}

	public void UpdateLives(string input)
	{
		bool result = int.TryParse(input, out _currentLives);
	}

	public void UpdateDefaultSpawnInterval(string input)
	{
		int spawnInterval = 0;
		bool result = int.TryParse(input, out spawnInterval);
		_currentDefaultSpawnInterval = (float) spawnInterval;
	}
	
	public void UpdateDefaultHitpoints(string input)
	{
		bool result = int.TryParse(input, out _currentDefaultHitpoints);
	}
	
	public void UpdateDefaultSpeed(string input)
	{
		int spawnInterval = 0;
		bool result = int.TryParse(input, out spawnInterval);
		_currentDefaultSpeed = (float) spawnInterval;
	}

	public void ValidateStep70()
	{
		string levelName = _levelData["Name"].str;
		LevelManager.SaveLevel(_levelData, levelName);
		JSONObject level = LevelManager.LoadLevel(levelName);
		Debug.Log(LevelManager.GetCustomLevels().ToString());

		SceneManager.LoadScene("Scenes/Menu/MainMenu");
	}
	
}
