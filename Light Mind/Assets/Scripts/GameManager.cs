using System.IO;
using System.Linq;
using Assets.Scripts.Utilities;
using Behaviors;
using Items;
using Models;
using UI;
using UnityEngine;
using UnityEngine.AI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Manager Scripts")] public BoardManager BoardManager;
    public TDManager TdManager;

    [Header("Containers")] public GameObject ItemsContainer;

    [Header("Items")] public GameObject MirrorPrefab;
    public GameObject FilterPrefab;
    public GameObject ObjectivePrefab;
    public GameObject ObstaclePrefab;
    public GameObject FilterMirrorPrefab;
    public GameObject LightSourcePrefab;
    public GameObject PrismPrefab;

    [Header("Inventory Items")] public GameObject MirrorInventoryItemPrefab;
    public GameObject FilterMirrorInventoryItemPrefab;
    public GameObject PrismInventoryItemPrefab;
    public GameObject FilterInventoryItemPrefab;

    [Header("UI")] public GameObject Inventory;
    public GameObject ColorPicker;
    public GameObject WinScreen;

    [Header("Tower Defense")] public bool IsTd;
    public GameObject EnemyPrefab;

    [Header("Turrets")] public GameObject StandardTurretPrefab;
    public GameObject MissileTurretPrefab;
    public GameObject LaserTurretPrefab;

    [Header("Turrets Inventory Items")] public GameObject StandardTurretInventoryItemPrefab;
    public GameObject MissileTurretInventoryItemPrefab;
    public GameObject LaserTurretInventoryItemPrefab;

    [Header("Enemy Path")] public GameObject SpawnerPrefab;
    public GameObject EnderPrefab;
    public GameObject PathPrefab;

    [Header("Enemy Navigation")] public NavMeshSurface NavigationSurface;

    private GameObject _selectedItem;

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
        TdManager = GetComponent<TDManager>();
    }

    private void Start()
    {
        var currentLevel = PlayerPrefs.GetString("currentLevel");
        if (!string.IsNullOrEmpty(currentLevel))
        {
            LoadLevel(currentLevel);
        }

        if (IsTd) TdManager.StartGame();
    }


    public void LoadLevel(string level)
    {
        var fileName = string.Format(IsTd ? "{0}_TD.json" : "{0}.json", level);
        string jsonText;
        var filePath = Path.Combine(Application.streamingAssetsPath, fileName);

        // Read JSON data from file
        if (Application.platform == RuntimePlatform.Android)
        {
            var www = new WWW(filePath);
            while (!www.isDone)
            {
            }

            jsonText = www.text;
        }
        else
        {
            jsonText = File.ReadAllText(filePath);
        }

        Debug.Log(jsonText);

        JSONObject dataAsJson = new JSONObject(jsonText);

        // Create board from file
        dataAsJson.GetField("Board", delegate(JSONObject boardData)
        {
            BoardManager.BoardSize.x = (int) boardData["Size"]["X"].i;
            BoardManager.BoardSize.y = (int) boardData["Size"]["Y"].i;

            BoardManager.CellSize = (int) boardData["CellSize"].i;
            BoardManager.CellOffset = (int) boardData["CellOffset"].i;

            BoardManager.SpawnPoint.x = (int) boardData["SpawnPoint"]["X"].i;
            BoardManager.SpawnPoint.y = (int) boardData["SpawnPoint"]["Y"].i;

            foreach (var path in boardData["Paths"].list)
            {
                BoardManager.Paths.Add(new BoardPath((int) path["X1"].i, (int) path["Y1"].i, (int) path["X2"].i,
                    (int) path["Y2"].i));
            }

            BoardManager.EndPoint.x = (int) boardData["EndPoint"]["X"].i;
            BoardManager.EndPoint.y = (int) boardData["EndPoint"]["Y"].i;

            BoardManager.CreateBoard();
        }, Debug.LogError);

        // Load player inventory
        if (dataAsJson["Inventory"]["Mirrors"].i > 0)
            CreateInventoryItem(MirrorInventoryItemPrefab, "mirror", (int) dataAsJson["Inventory"]["Mirrors"].i);

        if (dataAsJson["Inventory"]["MirrorFilters"].i > 0)
            CreateInventoryItem(FilterMirrorInventoryItemPrefab, "mirror-filter", (int) dataAsJson["Inventory"]["MirrorFilters"].i);

        if (dataAsJson["Inventory"]["Prisms"].i > 0)
            CreateInventoryItem(PrismInventoryItemPrefab, "prism", (int) dataAsJson["Inventory"]["Prisms"].i);

        if (dataAsJson["Inventory"]["Filters"].i > 0)
            CreateInventoryItem(FilterInventoryItemPrefab, "filter", (int) dataAsJson["Inventory"]["Filters"].i);

        if (IsTd)
        {
            if (dataAsJson["Inventory"]["StandardTurret"].i > 0)
                CreateInventoryItem(StandardTurretInventoryItemPrefab, "standard-turret", (int) dataAsJson["Inventory"]["StandardTurret"].i);

            if (dataAsJson["Inventory"]["MissileTurret"].i > 0)
                CreateInventoryItem(MissileTurretInventoryItemPrefab, "missile-turret", (int) dataAsJson["Inventory"]["MissileTurret"].i);

            if (dataAsJson["Inventory"]["LaserTurret"].i > 0)
                CreateInventoryItem(LaserTurretInventoryItemPrefab, "laser-turret", (int) dataAsJson["Inventory"]["LaserTurret"].i);
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
                    // Prism prism = objectInstance.GetComponentInChildren<Prism>();
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
                            new RayColor(jsonRay["Red"].b, jsonRay["Green"].b, jsonRay["Blue"].b,
                                RayColor.DEFAULT_ALPHA);
                        RaySource raySource =
                            new RaySource((Direction) jsonRay["Direction"].i, jsonRay["Enabled"].b, rayColor);
                        laser.AddSource(raySource);
                    }

                    break;

                case "Obstacle":
                    Debug.Log("Instanciating an obstacle...");
                    objectInstance = Instantiate(ObstaclePrefab, ItemsContainer.transform);
                    break;

                case "Standard Turret":
                    Debug.Log("Instanciating a standard turret...");
                    objectInstance = Instantiate(StandardTurretPrefab, ItemsContainer.transform);
                    break;

                case "Missile Turret":
                    Debug.Log("Instanciating a missile turret...");
                    objectInstance = Instantiate(MissileTurretPrefab, ItemsContainer.transform);
                    break;

                case "Laser Turret":
                    Debug.Log("Instanciating a laser turret...");
                    objectInstance = Instantiate(LaserTurretPrefab, ItemsContainer.transform);
                    break;

                default:
                    Debug.LogError(string.Format("Object of type {0} is not supported.", jsonEntity["Type"].str));
                    break;
            }

            if (objectInstance == null) continue;

            var objectInstancePosition = new Vector2Int((int) jsonEntity["X"].i, (int) jsonEntity["Y"].i);
            if (!BoardManager.AddItem(objectInstance, objectInstancePosition))
            {
                Debug.LogError(string.Format("Could not instantiate {0} at position {1}.", objectInstance,
                    objectInstancePosition));
                Destroy(objectInstance);
                continue;
            }

            var dragAndDrop = objectInstance.GetComponentInChildren<DragAndDrop>();
            if (dragAndDrop) dragAndDrop.IsDraggable = jsonEntity["Draggable"].b;

            var raySensitive = objectInstance.GetComponentInChildren<RaySensitive>();
            if (raySensitive) raySensitive.ColliderEnabled = true;
        }

        if (IsTd && dataAsJson != null)
        {
            TdManager.SetUpWaves(dataAsJson);
        }
    }

    private void CreateInventoryItem(GameObject itemPrefab, string itemCode, int count)
    {
        GameObject itemGameObject = Instantiate(itemPrefab, Inventory.transform);
        InventoryItem inventoryItem = itemGameObject.GetComponent<InventoryItem>();
        inventoryItem.ItemQuantity = count;
        inventoryItem.ItemCode = itemCode;
    }

    // Find the parent GameObject of all Quad Objects and get all objectives references
    private Objective[] GetAllObjectives()
    {
        return FindObjectsOfType<Objective>();
    }

    // Checks for a win condition (all objectives completed)
    public void CheckWinCondition()
    {
        Debug.Log("Checking Win conditions...");
        Objective[] objectives = GetAllObjectives();
        if (objectives.All(objective => objective.Completed))
        {
            Debug.LogWarning("Congratulations! All Objectives have been completed!");
            WinLevel();
        }
    }

    public void SelectItem(GameObject item)
    {
        if (_selectedItem != null && (item.GetInstanceID() == _selectedItem.GetInstanceID())) return;

        _selectedItem = item;

        var ib = _selectedItem.GetComponent<ItemBase>();

        if (ib == null) return;

        if (ib.IsColorable) ShowColorPanel();
        else HideColorPanel();
    }

    private void ShowColorPanel()
    {
        Inventory.SetActive(false);
        ColorPicker.SetActive(true);
    }

    public void HideColorPanel()
    {
        ColorPicker.SetActive(false);
        Inventory.SetActive(true);
    }

    public void SetSelectedItemColor(RayColor rayColor)
    {
        var ib = _selectedItem.GetComponent<ItemBase>();

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