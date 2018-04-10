using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection.Emit;
using Assets.Scripts.Utilities;
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
	public GameObject Step22;
	public GameObject Step30;
	public GameObject Step40;
	public GameObject Step50;
	public GameObject Step60;

	[Header("Debug")] 
	public int _currentLevelHeight;
	public int _currentLevelWidth;
	public Vector2Int[] _currentEnemyPath;
	public List<Vector2Int[]> _enemyPaths;
	public Vector2Int _spawnPoint;
	public Vector2Int _endPoint;
	public string _currentWave;
	public List<string> _waves;
	public int _currentStep;
	
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
		_enemyPaths = new List<Vector2Int[]>();
		_currentEnemyPath = new Vector2Int[2];
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
		_levelData["Board"]["SpawnPoint"]["X"].i = _spawnPoint[0];
		_levelData["Board"]["SpawnPoint"]["Y"].i = _spawnPoint[1];
		
		BoardManager.SpawnPoint.x = _spawnPoint[0];
		BoardManager.SpawnPoint.y = _spawnPoint[1];
		
		BoardManager.CreateBoardSpawner();
		
		Step20.SetActive(false);		
		Step21.SetActive(true);
		_currentStep = 21;
	}

	public void AddSpawnPoint(Vector3 clickPosition)
	{
		_spawnPoint = new Vector2Int(
			BoardManager.WorldToCellPosition(clickPosition.x),
			BoardManager.WorldToCellPosition(clickPosition.z)
		);		
	}
	
	/* Step 21 */

	public void ValideStep21()
	{
		_levelData["Board"]["EndPoint"]["X"].i = _endPoint[0];
		_levelData["Board"]["EndPoint"]["Y"].i = _endPoint[1];
		
		BoardManager.EndPoint.x = _endPoint[0];
		BoardManager.EndPoint.y = _endPoint[1];
		
		BoardManager.CreateBoardEnder();
		
		Step21.SetActive(false);		
		//Step22.SetActive(true);
		_currentStep = 22;
	}

	public void AddEndPoint(Vector3 clickPosition)
	{
		_endPoint = new Vector2Int(
			BoardManager.WorldToCellPosition(clickPosition.x),
			BoardManager.WorldToCellPosition(clickPosition.z)
		);		
	}
	
	/* Step 22 */

	public void ValideStep22()
	{
		foreach (var enemyPath in _enemyPaths)
		{
			JSONObject jsonPath = new JSONObject();
			jsonPath.AddField("X1", enemyPath[0][0]);
			jsonPath.AddField("Y1", enemyPath[0][1]);
			jsonPath.AddField("X2", enemyPath[1][0]);
			jsonPath.AddField("Y2", enemyPath[1][1]);

			_levelData["Board"]["Paths"].Add(jsonPath);
		}
	}

	public void AddPathStart(Vector3 clickPosition)
	{
		Vector2Int cellPosition = new Vector2Int(
			BoardManager.WorldToCellPosition(clickPosition.x),
			BoardManager.WorldToCellPosition(clickPosition.y)
			);
		_currentEnemyPath[0] = cellPosition;
	}
	
	public void AddPathEnd(Vector3 clickPosition)
	{
		Vector2Int cellPosition = new Vector2Int(
			BoardManager.WorldToCellPosition(clickPosition.x),
			BoardManager.WorldToCellPosition(clickPosition.y)
		);
		_currentEnemyPath[1] = cellPosition;
		_enemyPaths.Add((Vector2Int[]) _currentEnemyPath.Clone());
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
		Debug.Log("click");
		if (_currentStep == 20)
		{
			Vector3 screenPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			screenPoint.y = 2.0f;
			AddSpawnPoint(screenPoint);
		}
		if (_currentStep == 21)
		{
			Vector3 screenPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			screenPoint.y = 2.0f;
			AddEndPoint(screenPoint);
		}
	}
}
