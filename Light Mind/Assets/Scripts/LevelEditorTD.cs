using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection.Emit;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using Assets.Scripts.Utilities;
using Behaviors;
using Items;
using Models;
using UI;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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
	public GameObject ObstacleInventoryItemPrefab;
	
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
	public GameObject Step45;
	public GameObject Step50;
	public GameObject Step60;

	[Header("Debug")] 
	public GameObject _levelHeight;
	public GameObject _levelWidth;
	public BoardPath _currentEnemyPath;
	public List<BoardPath> _enemyPaths;
	public Vector2Int _spawnPoint;
	public Vector2Int _endPoint;
	public JSONObject _currentWave;
	public JSONObject _waves;
	public Text _currentWaveText;
	public Text _totalWavesText;
	public int _currentStep;
	public BoardCell _selectedCell;
	public GameObject _Filter;
	public GameObject _Mirror;
	public GameObject _Prism;
	public GameObject _FilterMirror;
	public GameObject _StandardTurret;
	public GameObject _MissileTurret;
	public GameObject _LaserTurret;
	public GameObject _Obstacle;
	public string _currentLevelName;
	public GameObject _currentLives;
	public GameObject _defaultSpawnInterval;
	public GameObject _defaultHitpoints;
	public GameObject _defaultSpeed;
	public string _currentDefaultColor;
	public RayColor _currentLightEastColor;
	public RayColor _currentLightNorthEastColor;
	public RayColor _currentLightNorthColor;
	public RayColor _currentLightNorthWestColor;
	public RayColor _currentLightWestColor;
	public RayColor _currentLightSouthWestColor;
	public RayColor _currentLightSouthColor;
	public RayColor _currentLightSouthEastColor;
	public RayColor[] _colorOptions;
	private GameObject _selectedItem;
	
	private JSONObject _levelData;
	
	private void Awake()
	{
		if (Instance == null)
			Instance = this;
		else if (Instance != this)
			Destroy(gameObject);
	}

    /// <summary>
    /// Use this for initialization of the level editor.
    /// </summary>
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
		_levelData["Inventory"].AddField("Obstacles", 0);
		
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
		_colorOptions = new RayColor[8];
		_colorOptions[0] = RayColor.NONE;
		_colorOptions[1] = RayColor.WHITE;
		_colorOptions[2] = RayColor.BLUE;
		_colorOptions[3] = RayColor.GREEN;
		_colorOptions[4] = RayColor.RED;
		_colorOptions[5] = RayColor.YELLOW;
		_colorOptions[6] = RayColor.CYAN;
		_colorOptions[7] = RayColor.MAGENTA;

		UpdateBoardHeight();
		UpdateBoardWidth();
		UpdateLives();
		UpdateDefaultSpawnInterval();
		UpdateDefaultHitpoints();
		UpdateDefaultSpeed();
		UpdateMirror();
		UpdatePrism();
		UpdateFilterMirror();
		UpdateStandardTurret();
		UpdateMissileTurret();
		UpdateLaserTurret();
		UpdateObstacle();

	}
	
	/* Step 10 */
    /// <summary>
    /// Load the first step to create a level.
    /// </summary>
	public void ValidateStep10()
	{
		_levelData["Board"]["Size"]["X"].i = (int) _levelWidth.GetComponentInChildren<Slider>().value;
		_levelData["Board"]["Size"]["Y"].i = (int) _levelHeight.GetComponentInChildren<Slider>().value;
		_levelData["Board"]["CellSize"].i = 4;
		_levelData["Board"]["CellOffset"].i = 1;
		
		BoardManager.BoardSize.x = (int) _levelWidth.GetComponentInChildren<Slider>().value;
		BoardManager.BoardSize.y = (int) _levelHeight.GetComponentInChildren<Slider>().value;

		BoardManager.CellSize = (int) _levelData["Board"]["CellSize"].i;
		BoardManager.CellOffset = (int) _levelData["Board"]["CellOffset"].i;

		BoardManager.CreateBoardCells();

		Step10.SetActive(false);

		BoardManager.AdjustCamera();
		
		Step60.SetActive(true);
		_currentStep = 60;
	}

    /// <summary>
    /// Change the height of the board.
    /// </summary>
	public void UpdateBoardHeight()
	{
		_levelHeight.GetComponentInChildren<Text>().text = String.Format("Height: {0}", _levelHeight.GetComponentInChildren<Slider>().value);
	}
	
    /// <summary>
    /// Change the width of the board.
    /// </summary>
	public void UpdateBoardWidth()
	{
		_levelWidth.GetComponentInChildren<Text>().text = String.Format("Width: {0}", _levelWidth.GetComponentInChildren<Slider>().value);
	}

    /* Step 20 */
    /// <summary>
    /// Load the first part of the second step to create a level.
    /// </summary>
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
    /// <summary>
    /// Load the second part of the second step to create a level.
    /// </summary>
    public void ValideStep21()
	{
		if (_selectedCell == null) return;
		
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
    /// <summary>
    /// Load the third part of the second step to create a level.
    /// </summary>
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
		if (_selectedCell == null) return;
		
		_currentEnemyPath.Start = _selectedCell.GetPosition();
		ResetSelectedCell();
		
		Debug.Log("Validating Step 220");
		Step220.SetActive(false);
		Step221.SetActive(true);
		_currentStep = 221;
	}
	
	public void ValidateStep221()
	{
		if (_selectedCell == null) return;
		
		_currentEnemyPath.End = _selectedCell.GetPosition();
		ResetSelectedCell();

		if (_currentEnemyPath.Start.x >= _currentEnemyPath.End.x
		    && _currentEnemyPath.Start.y >= _currentEnemyPath.End.y)
		{
			Vector2Int buf = new Vector2Int(_currentEnemyPath.Start.x, _currentEnemyPath.Start.y);
			_currentEnemyPath.Start.x = _currentEnemyPath.End.x;
			_currentEnemyPath.Start.y = _currentEnemyPath.End.y;
			_currentEnemyPath.End.x = buf.x;
			_currentEnemyPath.End.y = buf.y;
		}
		
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
    /// <summary>
    /// Apply the color to the path selected.
    /// </summary>
    /// <param name="boardPath">A matrix representif the new path for ennemies.</param>
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
					Debug.Log(cell);
					cell.SelectCell();
				}
			}
		}
	}

    /* Step 30 */
    /// <summary>
    /// Load the third step to create a level.
    /// </summary>
    public void ValidateStep30()
	{
		if (_waves.Count <= 0) return;
		
		_levelData["Waves"] = _waves;
		
		Step40.SetActive(true);
		Step30.SetActive(false);

		PrepareStep40();
		
		_currentStep = 40;
	}

    /// <summary>
    /// The number of a type of ennemy to add to a wave.
    /// </summary>
    /// <param name="enemyCode">Represents the type of ennemy.</param>
	public void AddEnemyToWave(string enemyCode)
	{
		JSONObject enemy = new JSONObject();
		enemy.AddField("Hitpoints", (int) _defaultHitpoints.GetComponentInChildren<Slider>().value);
		enemy.AddField("Speed", _defaultSpeed.GetComponentInChildren<Slider>().value);
		enemy.AddField("Color", enemyCode);
		_currentWave["Enemies"].Add(enemy);
		UpdateCurrentWaveText();
	}
    
    /// <summary>
    /// Prints the number of ennemy to the wave.
    /// </summary>
	private void UpdateCurrentWaveText()
	{
		int red = 0;
		int blue = 0;
		int yellow = 0;
		int cyan = 0;
		int green = 0;
		int white = 0;
		int magenta = 0;
		int none = 0;

		foreach (JSONObject enemy in _currentWave["Enemies"].list)
		{
			if (enemy["Color"].str == "Red") red++;
			if (enemy["Color"].str == "Blue") blue++;
			if (enemy["Color"].str == "Yellow") yellow++;
			if (enemy["Color"].str == "Cyan") cyan++;
			if (enemy["Color"].str == "Green") green++;
			if (enemy["Color"].str == "White") white++;
			if (enemy["Color"].str == "Magenta") magenta++;
			if (enemy["Color"].str == "None") none++;
		}

		StringBuilder sb = new StringBuilder();
		if (red > 0) sb.Append("<size=30><color=red>●</color></size> x").Append(red).Append(Environment.NewLine);
		if (blue > 0) sb.Append("<size=30><color=blue>●</color></size> x").Append(blue).Append(Environment.NewLine);
		if (yellow > 0) sb.Append("<size=30><color=yellow>●</color></size> x").Append(yellow).Append(Environment.NewLine);
		if (cyan > 0) sb.Append("<size=30><color=cyan>●</color></size> x").Append(cyan).Append(Environment.NewLine);
		if (green > 0) sb.Append("<size=30><color=green>●</color></size> x").Append(green).Append(Environment.NewLine);
		if (white > 0) sb.Append("<size=30><color=white>●</color></size> x").Append(white).Append(Environment.NewLine);
		if (magenta > 0) sb.Append("<size=30><color=magenta>●</color></size> x").Append(magenta).Append(Environment.NewLine);
		if (none > 0) sb.Append("<size=30><color=black>●</color></size> x").Append(none).Append(Environment.NewLine);
		_currentWaveText.text = String.Format("Current wave:{0}{1}", Environment.NewLine, sb.ToString());
	}

    /// <summary>
    /// Prints the number of waves to be played.
    /// </summary>
	private void UpdateTotalWavesText()
	{
		_totalWavesText.text = String.Format("Total: {0} wave(s)", _waves.Count);
	}
	
    /// <summary>
    /// Add a wave.
    /// </summary>
	public void AddWave()
	{
		string waveString = _currentWave.ToString();
		Debug.Log(waveString);
		JSONObject waveCopy = new JSONObject(waveString);
		Debug.Log(waveCopy);
		_waves.Add(waveCopy);
		_currentWave = new JSONObject();
		_currentWave.AddField("Enemies", new JSONObject(JSONObject.Type.ARRAY));
		UpdateTotalWavesText();
		UpdateCurrentWaveText();
	}

	public void OnPointerClick(PointerEventData eventData)
	{
		SelectCell(eventData);
	}

    /// <summary>
    /// Select a cell to create a path to ennemies.
    /// </summary>
    /// <param name="eventData"></param>
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
    
    /// <summary>
    /// Reset a selected cell.
    /// </summary>
	private void ResetSelectedCell()
	{
		_selectedCell.ResetCellColor();
		_selectedCell = null;
		Debug.Log(_selectedCell);
	}

    /// <summary>
    /// Translate a 3 dimensional view to a 2 dimensional view.
    /// </summary>
    /// <param name="worldVector"></param>
    /// <returns></returns>
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
	
	public void GoToStep45()
	{
		Step45.SetActive(true);
		Step40.SetActive(false);
	}

	public void ValidateStep45()
	{
		BoardCell cell = FindAvailableCell();
		if (cell != null
		    && (_currentLightEastColor != RayColor.NONE
		        || _currentLightNorthEastColor != RayColor.NONE
		        || _currentLightNorthColor != RayColor.NONE
		        || _currentLightNorthWestColor != RayColor.NONE
		        || _currentLightWestColor != RayColor.NONE
		        || _currentLightSouthWestColor != RayColor.NONE
		        || _currentLightSouthColor != RayColor.NONE
		        || _currentLightSouthEastColor != RayColor.NONE
		    ))
		{
			GameObject laserGameObject = Instantiate(LightSourcePrefab, ItemsContainer.transform);
			cell.AddItem(laserGameObject);
			Laser laser = laserGameObject.GetComponent<Laser>();
			laser.IsColorable = false;
			laser.IsOrientable = false;
			AddRayToLightSource(laser, _currentLightEastColor, Direction.East);
			AddRayToLightSource(laser, _currentLightNorthEastColor, Direction.NorthEast);
			AddRayToLightSource(laser, _currentLightNorthColor, Direction.North);
			AddRayToLightSource(laser, _currentLightNorthWestColor, Direction.NorthWest);
			AddRayToLightSource(laser, _currentLightWestColor, Direction.West);
			AddRayToLightSource(laser, _currentLightSouthWestColor, Direction.SouthWest);
			AddRayToLightSource(laser, _currentLightSouthColor, Direction.South);
			AddRayToLightSource(laser, _currentLightSouthEastColor, Direction.SouthEast);
			
			DragAndDrop dragAndDrop = laserGameObject.GetComponent<DragAndDrop>();
			dragAndDrop.IsDraggable = true;
		}

		Inventory.SetActive(false);
		Step45.SetActive(false);
		Step40.SetActive(true);
	}

    /// <summary>
    /// Add a ray to the light source on the board.
    /// </summary>
    /// <param name="laser">Represents the laser to add.</param>
    /// <param name="color">The color of the laser.</param>
    /// <param name="direction">The direction of the laser from the light source.</param>
	private void AddRayToLightSource(Laser laser, RayColor color, Direction direction)
	{
		laser.AddSource(new RaySource(direction, color != RayColor.NONE, color));
	}

	public BoardCell FindAvailableCell()
	{
		foreach (BoardCell boardCell in BoardManager.Board.GetComponentsInChildren<BoardCell>())
		{
			if (!boardCell.IsOccupied()) return boardCell;
		}

		return null;
	}

	public void UpdateLightEastColor(Int32 value)
	{
		_currentLightEastColor = _colorOptions[value];
	}
	
	public void UpdateLightNorthEastColor(Int32 value)
	{
		_currentLightNorthEastColor = _colorOptions[value];
	}
	
	public void UpdateLightNorthColor(Int32 value)
	{
		_currentLightNorthColor = _colorOptions[value];
	}
	
	public void UpdateLightNorthWestColor(Int32 value)
	{
		_currentLightNorthWestColor = _colorOptions[value];
	}
	
	public void UpdateLightWestColor(Int32 value)
	{
		_currentLightWestColor = _colorOptions[value];
	}
	
	public void UpdateLightSouthWestColor(Int32 value)
	{
		_currentLightSouthWestColor = _colorOptions[value];
	}
	
	public void UpdateLightSouthColor(Int32 value)
	{
		_currentLightSouthColor = _colorOptions[value];
	}
	
	public void UpdateLightSouthEastColor(Int32 value)
	{
		_currentLightSouthEastColor = _colorOptions[value];
	}
	
	// Load player inventory
    /// <summary>
    /// Load a full inventory (99 items).
    /// </summary>
	private void LoadFullInventory()
	{
		CreateInventoryItem(MirrorInventoryItemPrefab, "mirror", 99);
		CreateInventoryItem(FilterMirrorInventoryItemPrefab, "mirror-filter", 99);
		CreateInventoryItem(PrismInventoryItemPrefab, "prism", 99);
		CreateInventoryItem(FilterInventoryItemPrefab, "filter", 99);
		CreateInventoryItem(StandardTurretInventoryItemPrefab, "standard-turret", 99);
		CreateInventoryItem(MissileTurretInventoryItemPrefab, "missile-turret", 99);
		CreateInventoryItem(LaserTurretInventoryItemPrefab, "laser-turret", 99);
		CreateInventoryItem(ObstacleInventoryItemPrefab, "obstacle", 99);
	}
	
    /// <summary>
    /// Create the inventory to the level to create.
    /// </summary>
    /// <param name="itemPrefab">The prefab of the inventory.</param>
    /// <param name="itemCode">The code representing the item.</param>
    /// <param name="count">The number of item.</param>
	private void CreateInventoryItem(GameObject itemPrefab, string itemCode, int count)
	{
		GameObject itemGameObject = Instantiate(itemPrefab, Inventory.transform);
		InventoryItem inventoryItem = itemGameObject.GetComponent<InventoryItem>();
		inventoryItem.ItemQuantity = count;
		inventoryItem.ItemCode = itemCode;
	}

    /// <summary>
    /// Save all the items.
    /// </summary>
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

    /// <summary>
    /// Save an item.
    /// </summary>
    /// <param name="item">Item to be saved.</param>
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

    /// <summary>
    /// Save a mirror and its orientation.
    /// </summary>
    /// <param name="item"></param>
    /// <param name="jsonObject"></param>
	private void SaveMirrorItem(Transform item, JSONObject jsonObject)
	{
		Mirror mirror = item.GetComponent<Mirror>();
		jsonObject.AddField("Type", "Mirror");
		jsonObject.AddField("Orientation", (int) mirror.Orientation);		
	}
	
    /// <summary>
    /// Save a filter and its color.
    /// </summary>
    /// <param name="item"></param>
    /// <param name="jsonObject"></param>
	private void SaveFilterItem(Transform item, JSONObject jsonObject)
	{
		Filter filter = item.GetComponent<Filter>();
		jsonObject.AddField("Type", "Filter");
		SaveItemColor(jsonObject, filter.Color);
	}

    /// <summary>
    /// Save a prism and its color.
    /// </summary>
    /// <param name="item"></param>
    /// <param name="jsonObject"></param>
	private void SavePrismItem(Transform item, JSONObject jsonObject)
	{
		Prism prism = item.GetComponent<Prism>();
		jsonObject.AddField("Type", "Prism");
		SaveItemColor(jsonObject, prism.Color);
	}

    /// <summary>
    /// Save a filter mirror with its orientation and color.
    /// </summary>
    /// <param name="item"></param>
    /// <param name="jsonObject"></param>
	private void SaveFilterMirrorItem(Transform item, JSONObject jsonObject)
	{
		FilterMirror filterMirror = item.GetComponent<FilterMirror>();
		jsonObject.AddField("Type", "Filter Mirror");
		SaveItemColor(jsonObject, filterMirror.Color);
		jsonObject.AddField("Orientation", (int) filterMirror.Orientation);
	}

    /// <summary>
    /// Save an obstacle.
    /// </summary>
    /// <param name="item"></param>
    /// <param name="jsonObject"></param>
	private void SaveObstacleItem(Transform item, JSONObject jsonObject)
	{
		jsonObject.AddField("Type", "Obstacle");
	}
	
    /// <summary>
    /// Save a turret.
    /// </summary>
    /// <param name="item"></param>
    /// <param name="jsonObject"></param>
	private void SaveStandardTurretItem(Transform item, JSONObject jsonObject)
	{
		jsonObject.AddField("Type", "Standard Turret");
	}
	
    /// <summary>
    /// Save a missile turret.
    /// </summary>
    /// <param name="item"></param>
    /// <param name="jsonObject"></param>
	private void SaveMissileTurretItem(Transform item, JSONObject jsonObject)
	{
		jsonObject.AddField("Type", "Missile Turret");
	}
	
    /// <summary>
    /// Save a laser turret.
    /// </summary>
    /// <param name="item"></param>
    /// <param name="jsonObject"></param>
	private void SaveLaserTurretItem(Transform item, JSONObject jsonObject)
	{
		jsonObject.AddField("Type", "Laser Turret");
	}

    /// <summary>
    /// Save a light source with its colors and orientation.
    /// </summary>
    /// <param name="item"></param>
    /// <param name="jsonObject"></param>
	private void SaveLightSourceItem(Transform item, JSONObject jsonObject)
	{
		Laser laser = item.GetComponent<Laser>();
		jsonObject.AddField("Type", "Light Source");
		jsonObject.AddField("Rays", new JSONObject(JSONObject.Type.ARRAY));
		foreach (RaySource source in laser.Sources)
		{
			JSONObject ray = new JSONObject();
			ray.AddField("Direction", (int) source.Direction);
			ray.AddField("Enabled", source.Enabled);
			ray.AddField("Red", source.Color.R);
			ray.AddField("Blue", source.Color.B);
			ray.AddField("Green", source.Color.G);
			jsonObject["Rays"].Add(ray);
		}
	}

    /// <summary>
    /// Save the color of the set item.
    /// </summary>
    /// <param name="jsonObject"></param>
    /// <param name="color"></param>
	private void SaveItemColor(JSONObject jsonObject, RayColor color)
	{
		jsonObject.AddField("Red", color.R);
		jsonObject.AddField("Green", color.G);
		jsonObject.AddField("Blue", color.B);
	}
	
	public void ValidateStep50()
	{
		_levelData["Inventory"]["Mirrors"].i = (int) _Mirror.GetComponentInChildren<Slider>().value;
		_levelData["Inventory"]["Filters"].i = (int) _Filter.GetComponentInChildren<Slider>().value;
		_levelData["Inventory"]["Prisms"].i = (int) _Prism.GetComponentInChildren<Slider>().value;
		_levelData["Inventory"]["MirrorFilters"].i = (int) _FilterMirror.GetComponentInChildren<Slider>().value;
		_levelData["Inventory"]["StandardTurret"].i = (int) _StandardTurret.GetComponentInChildren<Slider>().value;
		_levelData["Inventory"]["MissileTurret"].i = (int) _MissileTurret.GetComponentInChildren<Slider>().value;
		_levelData["Inventory"]["LaserTurret"].i = (int) _LaserTurret.GetComponentInChildren<Slider>().value;
		_levelData["Inventory"]["Obstacles"].i = (int) _Obstacle.GetComponentInChildren<Slider>().value;
		
		Step50.SetActive(false);	
		_currentStep = 70;
		ValidateStep70();
		
		Debug.Log(_levelData.ToString());
	}

    /// <summary>
    /// Update the mirror to the board.
    /// </summary>
	public void UpdateMirror()
	{
		_Mirror.GetComponentInChildren<Text>().text = String.Format("Mirrors: {0}", _Mirror.GetComponentInChildren<Slider>().value);
	}

    /// <summary>
    /// Update the filter to the board.
    /// </summary>
	public void UpdateFilter()
	{
		_Filter.GetComponentInChildren<Text>().text = String.Format("Filters: {0}", _Filter.GetComponentInChildren<Slider>().value);
	}

    /// <summary>
    /// Update the prism to the board.
    /// </summary>
	public void UpdatePrism()
	{
		_Prism.GetComponentInChildren<Text>().text = String.Format("Prisms: {0}", _Prism.GetComponentInChildren<Slider>().value);
	}

    /// <summary>
    /// Update the filter mirror to the board.
    /// </summary>
	public void UpdateFilterMirror()
	{
		_FilterMirror.GetComponentInChildren<Text>().text = String.Format("Filter-mirrors: {0}", _FilterMirror.GetComponentInChildren<Slider>().value);
	}

    /// <summary>
    /// Update the turret to the board.
    /// </summary>
	public void UpdateStandardTurret()
	{
		_StandardTurret.GetComponentInChildren<Text>().text = String.Format("Standard turrets: {0}", _StandardTurret.GetComponentInChildren<Slider>().value);
	}

    /// <summary>
    /// Update the missile turret to the board.
    /// </summary>
	public void UpdateMissileTurret()
	{
		_MissileTurret.GetComponentInChildren<Text>().text = String.Format("Missile turrets: {0}", _MissileTurret.GetComponentInChildren<Slider>().value);
	}

    /// <summary>
    /// Update the laser turret to the board.
    /// </summary>
	public void UpdateLaserTurret()
	{
		_LaserTurret.GetComponentInChildren<Text>().text = String.Format("Laser turrets: {0}", _LaserTurret.GetComponentInChildren<Slider>().value);
	}
	
    /// <summary>
    /// Update the obstacle to the board.
    /// </summary>
	public void UpdateObstacle()
	{
		_Obstacle.GetComponentInChildren<Text>().text = String.Format("Ostacles: {0}", _Obstacle.GetComponentInChildren<Slider>().value);
	}

	public void ValidateStep60()
	{
		if (_currentLevelName.Length <= 0) return;
		
		_levelData["Name"].str = _currentLevelName;
		_levelData["FunFact"].str = "Savez-vous que lorsque vous créez un niveau via l'éditeur, vous ne pourrez plus jamais le modifier, ni le supprimer ?";
		_levelData["Info"]["Lives"].i = (int) _currentLives.GetComponentInChildren<Slider>().value;
		_levelData["Info"]["DefaultSpawnInterval"].n = _defaultSpawnInterval.GetComponentInChildren<Slider>().value;
		_levelData["Info"]["DefaultHitpoints"].i = (int) _defaultHitpoints.GetComponentInChildren<Slider>().value;
		_levelData["Info"]["DefaultSpeed"].n = _defaultSpeed.GetComponentInChildren<Slider>().value;
		_levelData["Info"]["DefaultColor"].str = "White";
	
		Step60.SetActive(false);
		Step20.SetActive(true);

	}
	
    /// <summary>
    /// Update the name of the level.
    /// </summary>
    /// <param name="input"></param>
	public void UpdateLevelName(string input)
	{
		_currentLevelName = input;
	}

    /// <summary>
    /// Update the number of lives of the player.
    /// </summary>
	public void UpdateLives()
	{
		_currentLives.GetComponentInChildren<Text>().text = String.Format("Lives: {0}", _currentLives.GetComponentInChildren<Slider>().value);
	}

    /// <summary>
    /// Update the time between the spawns of ennemies.
    /// </summary>
	public void UpdateDefaultSpawnInterval()
	{
		_defaultSpawnInterval.GetComponentInChildren<Text>().text = String.Format("Spawn interval: {0}", _defaultSpawnInterval.GetComponentInChildren<Slider>().value);
	}
	
    /// <summary>
    /// Update the life of ennemies.
    /// </summary>
	public void UpdateDefaultHitpoints()
	{
		_defaultHitpoints.GetComponentInChildren<Text>().text = String.Format("Enemies life: {0}", _defaultHitpoints.GetComponentInChildren<Slider>().value);
	}
	
    /// <summary>
    /// Update the default speed to a level.
    /// </summary>
	public void UpdateDefaultSpeed()
	{
		_defaultSpeed.GetComponentInChildren<Text>().text = String.Format("Enemies speed: {0}", _defaultSpeed.GetComponentInChildren<Slider>().value);
	}

	public void ValidateStep70()
	{		
		string levelName = _levelData["Name"].str;
		LevelManager.SaveLevel(_levelData, levelName);
		JSONObject level = LevelManager.LoadLevel(levelName);
		Debug.Log(LevelManager.GetCustomLevels().ToString());

		SceneManager.LoadScene("Scenes/Menu/MainMenu");
	}
	
    /// <summary>
    /// Select an item to add.
    /// </summary>
    /// <param name="item">Represents the item.</param>
	public void SelectItem(GameObject item)
	{
		if (_selectedItem != null && (item.GetInstanceID() == _selectedItem.GetInstanceID())) return;

		_selectedItem = item;

		var ib = _selectedItem.GetComponent<ItemBase>();

		if (ib == null) return;

		if (ib.IsColorable) ShowColorPanel();
		else HideColorPanel();
	}
    

    /// <summary>
    /// Instantiate the color panel.
    /// </summary>
	private void ShowColorPanel()
	{
		Inventory.SetActive(false);
		ColorPicker.SetActive(true);
	}

    /// <summary>
    /// Hide the color panel.
    /// </summary>
	public void HideColorPanel()
	{
		ColorPicker.SetActive(false);
		Inventory.SetActive(true);
	}

    /// <summary>
    /// Apply a ray color to an item.
    /// </summary>
    /// <param name="rayColor">Color of the ray.</param>
	public void SetSelectedItemColor(RayColor rayColor)
	{
		var ib = _selectedItem.GetComponent<ItemBase>();

		if (ib != null)
		{
			ib.SetColor(rayColor);
		}

		_selectedItem = null;
	}
	
}
