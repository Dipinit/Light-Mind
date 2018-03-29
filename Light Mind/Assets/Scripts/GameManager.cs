using System;
using System.IO;
using System.Linq;
using Assets.Scripts.Utilities;
using Behaviors;
using Items;
using UI;
using UnityEngine;
using DragAndDrop = Behaviors.DragAndDrop;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public BoardManager BoardManager;
    public GameObject ItemsContainer;

    public GameObject MirrorPrefab;
    public GameObject FilterPrefab;
    public GameObject ObjectivePrefab;
    public GameObject ObstaclePrefab;
    public GameObject FilterMirrorPrefab;
    public GameObject LightSourcePrefab;
    public GameObject PrismPrefab;

    public GameObject MirrorInventoryItemPrefab;
    public GameObject FilterMirrorInventoryItemPrefab;
    public GameObject PrismInventoryItemPrefab;
    public GameObject FilterInventoryItemPrefab;
    public GameObject Inventory;
    public GameObject ColorPicker;
    public GameObject WinScreen;

    private GameObject _selectedItem = null;

	// TD variables
    public TDManager TDManager;
	public Boolean isTD = false;

	public GameObject EnemyPrefab;
	public GameObject TowerPrefab;
	public GameObject PathPrefab;
	public GameObject TowerInventoryPrefab;
	public GameObject SpawnerPrefab;
	public GameObject EnderPrefab;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else if (Instance != this)
            Destroy(gameObject);

        // Sets this to not be destroyed when reloading scene
        // Use this if you wanna keep track of game score between levels for example.
        // Be careful, you need to manage item instanciation carefully if this is enabled!
        // DontDestroyOnLoad(gameObject);

        // Create items container
        if (null == ItemsContainer)
        {
            ItemsContainer = new GameObject {name = "Items"};
        }

        BoardManager = GetComponent<BoardManager>();
    }

    private void Start()
    {
        BoardManager.CreateBoard();

        string currentLevel = PlayerPrefs.GetString("currentLevel");
        if (!String.IsNullOrEmpty(currentLevel))
        {
            LoadLevel(currentLevel);
        }

        if (isTD) TDManager.StartGame ();
    }
    

    public void LoadLevel(string level)
    {
		string fileName;
		if (isTD) {
			fileName = string.Format("{0}_TD.json", level);
		} else {
        	fileName = string.Format("{0}.json", level);
		}
        string jsonText = null;
        string filePath = System.IO.Path.Combine(Application.streamingAssetsPath, fileName);


        if (Application.platform == RuntimePlatform.Android)
        {
            WWW www = new WWW(filePath);
            while (!www.isDone) {};
            jsonText = www.text;
        }
        else 
        {
            jsonText = File.ReadAllText(filePath);
        }
        
        // Read the json from the file into a string
        Debug.Log(jsonText);

        JSONObject dataAsJson = new JSONObject(jsonText);

        if (dataAsJson["Inventory"]["Mirrors"].i > 0)
        {
            GameObject itemGameObject = Instantiate(MirrorInventoryItemPrefab, Inventory.transform);
            InventoryItem inventoryItem = itemGameObject.GetComponent<InventoryItem>();
            inventoryItem.ItemQuantity = (int) dataAsJson["Inventory"]["Mirrors"].i;
        }

        if (dataAsJson["Inventory"]["MirrorFilters"].i > 0)
        {
            GameObject itemGameObject = Instantiate(FilterMirrorInventoryItemPrefab, Inventory.transform);
            InventoryItem inventoryItem = itemGameObject.GetComponent<InventoryItem>();
            inventoryItem.ItemQuantity = (int) dataAsJson["Inventory"]["MirrorFilters"].i;
        }

        if (dataAsJson["Inventory"]["Prisms"].i > 0)
        {
            GameObject itemGameObject = Instantiate(PrismInventoryItemPrefab, Inventory.transform);
            InventoryItem inventoryItem = itemGameObject.GetComponent<InventoryItem>();
            inventoryItem.ItemQuantity = (int) dataAsJson["Inventory"]["Prisms"].i;
        }

		if (dataAsJson["Inventory"]["Filters"].i > 0)
        {
            GameObject itemGameObject = Instantiate(FilterInventoryItemPrefab, Inventory.transform);
            InventoryItem inventoryItem = itemGameObject.GetComponent<InventoryItem>();
            inventoryItem.ItemQuantity = (int) dataAsJson["Inventory"]["Filters"].i;
        }

		// ADD TOWERS
		if (isTD && dataAsJson["Inventory"]["Towers"].i > 0)
		{
			GameObject itemGameObject = Instantiate(TowerInventoryPrefab, Inventory.transform);
			InventoryItem inventoryItem = itemGameObject.GetComponent<InventoryItem>();
			inventoryItem.ItemQuantity = (int) dataAsJson["Inventory"]["Towers"].i;
		}

        foreach (var jsonEntity in dataAsJson["Entities"].list)
        {
            GameObject objectInstance = null;
            switch (jsonEntity["Type"].str)
            {
                case "Mirror":
                    Debug.Log("Instanciating a mirror...");
                    objectInstance = Instantiate(MirrorPrefab, ItemsContainer.transform);
                    Mirror mirror = objectInstance.GetComponentInChildren<Mirror>();
                    mirror.Orientation = (Direction) jsonEntity["Orientation"].i;
                    break;

                case "Filter":
                    Debug.Log("Instanciating a filter...");
                    objectInstance = Instantiate(FilterPrefab, ItemsContainer.transform);
                    Filter filter = objectInstance.GetComponentInChildren<Filter>();
                    filter.Color = new RayColor(
                        jsonEntity["Red"].b,
                        jsonEntity["Green"].b,
                        jsonEntity["Blue"].b,
                        RayColor.DEFAULT_ALPHA);
                    break;

                case "Objective":
                    Debug.Log("Instanciating an objective...");
                    objectInstance = Instantiate(ObjectivePrefab, ItemsContainer.transform);
                    objectInstance.tag = "objective";
                    Objective objective = objectInstance.GetComponentInChildren<Objective>();
                    objective.Color = new RayColor(
                        jsonEntity["Red"].b,
                        jsonEntity["Green"].b,
                        jsonEntity["Blue"].b,
                        RayColor.DEFAULT_ALPHA);
                    break;

                case "Prism":
                    Debug.Log("Instanciating a prism...");
                    objectInstance = Instantiate(PrismPrefab, ItemsContainer.transform);
                    Prism prism = objectInstance.GetComponentInChildren<Prism>();
                    break;

                case "Filter Mirror":
                    Debug.Log("Instanciating a filter mirror...");
                    objectInstance = Instantiate(FilterMirrorPrefab, ItemsContainer.transform);
                    FilterMirror filterMirror = objectInstance.GetComponentInChildren<FilterMirror>();
                    filterMirror.Orientation = (Direction) jsonEntity["Orientation"].i;
                    filterMirror.Color = new RayColor(
                        jsonEntity["Red"].b,
                        jsonEntity["Green"].b,
                        jsonEntity["Blue"].b,
                        RayColor.DEFAULT_ALPHA);
                    break;

                case "Light Source":
                    Debug.Log("Instanciating a light source...");
                    objectInstance = Instantiate(LightSourcePrefab, ItemsContainer.transform);
                    Laser laser = objectInstance.GetComponentInChildren<Laser>();
                    foreach (var jsonRay in jsonEntity["Rays"].list)
                    {
                        RayColor rayColor =
                            new RayColor(jsonRay["Red"].b, jsonRay["Green"].b, jsonRay["Blue"].b, RayColor.DEFAULT_ALPHA);
                        RaySource raySource =
                            new RaySource((Direction) jsonRay["Direction"].i, jsonRay["Enabled"].b, rayColor);
                        laser.AddSource(raySource);
                    }
                    break;
                case "Obstacle":
                    Debug.Log("Instanciating an obstacle...");
                    objectInstance = Instantiate(ObstaclePrefab, ItemsContainer.transform);
                    break;
				// ADD TOWER
				case "Tower":
					Debug.Log("Instanciating a tower...");
					objectInstance = Instantiate(TowerPrefab, ItemsContainer.transform);
					break;
				// ADD SPAWN
				case "Spawner":
					Debug.Log("Instanciating a spawner...");
					objectInstance = Instantiate(SpawnerPrefab, ItemsContainer.transform);
					break;
				// ADD END
				case "Ender":
					Debug.Log("Instanciating an ender...");
					objectInstance = Instantiate(EnderPrefab, ItemsContainer.transform);
					break;
				// ADD PATH
				case "Path":
					Debug.Log("Instanciating a path...");
                    foreach(var jsonPath in jsonEntity["Paths"].list) {
                        objectInstance = Instantiate(PathPrefab, ItemsContainer.transform);
                        Vector3 posPath = new Vector3(jsonPath["X"].n, jsonPath["Y"].n, 0);
                        TDManager.AddPath (posPath);
                        objectInstance.transform.position = posPath;
                        BoardManager.AddItemPosition (posPath);
                    }
					continue;
                default:
                    Debug.LogError(string.Format("Object of type {0} is not supported.", jsonEntity["Type"].str));
                    break;
            }

            if (objectInstance == null) continue;

            Vector3 pos = new Vector3(jsonEntity["X"].n, jsonEntity["Y"].n, 0);
            objectInstance.transform.position = pos;
            DragAndDrop dragAndDrop = objectInstance.GetComponentInChildren<DragAndDrop>();
            if (dragAndDrop) dragAndDrop.IsDraggable = jsonEntity ["Draggable"].b;
            RaySensitive raySensitive = objectInstance.GetComponentInChildren<RaySensitive>();
            if (raySensitive) raySensitive.ColliderEnabled = true;
            BoardManager.AddItemPosition (pos);
        }

        if (isTD && dataAsJson != null) {
            TDManager.SetUpWaves (dataAsJson);
        }
    }

    // Find the parent GameObject of all Quad Objects and get all objectives references
	private Objective[] GetAllObjectives()
    {
		return FindObjectsOfType<Objective> ();
    }

    // Checks for a win condition (all objectives completed)
    public void CheckWinCondition()
    {
        Debug.Log("Checking Win conditions...");
		Objective[] objectives = GetAllObjectives ();
		if (objectives.All(objective => objective.Completed))
		{
			Debug.LogWarning("Congratulations! All Objectives have been completed!");
            WinLevel();
        }
    }

    public void SelectItem(GameObject item)
    {
        if (_selectedItem == null || (item.GetInstanceID() != _selectedItem.GetInstanceID()))
        {
            _selectedItem = item;

            ItemBase ib = _selectedItem.GetComponent<ItemBase>();

            if (ib != null)
            {
                if (ib.IsColorable) ShowColorPanel();
                else HideColorPanel();
            }
        }
    }

    private void ShowColorPanel()
    {
        this.Inventory.SetActive(false);
        this.ColorPicker.SetActive(true);

    }

    public void HideColorPanel()
    {
        this.ColorPicker.SetActive(false);
        this.Inventory.SetActive(true);
    }

    public void SetSelectedItemColor(RayColor rayColor)
    {
        ItemBase ib = _selectedItem.GetComponent<ItemBase>();

        if (ib != null)
        {
            ib.SetColor(rayColor);
        }

        _selectedItem = null;
    }
    

    public void WinLevel()
    {
        WinScreen.SetActive(true);
    }
}