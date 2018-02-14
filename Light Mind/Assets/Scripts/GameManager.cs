using System;
using System.IO;
using System.Linq;
using Assets.Scripts.Utilities;
using Behaviors;
using Items;
using UI;
using UnityEngine;

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
    public GameObject WinScreen;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else if (Instance != this)
            Destroy(gameObject);

        //Sets this to not be destroyed when reloading scene
        DontDestroyOnLoad(gameObject);

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
            LoadLevel(PlayerPrefs.GetString("currentLevel"));
            GetAllObjectives();
        }
    }

    public void LoadLevel(string level)
    {
        string filePath = Path.Combine(Application.streamingAssetsPath, string.Format("{0}.json", level));
        if (File.Exists(filePath))
        {
            // Read the json from the file into a string
            string jsonText = File.ReadAllText(filePath);
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
                        filter.Red = jsonEntity["Red"].b;
                        filter.Green = jsonEntity["Green"].b;
                        filter.Blue = jsonEntity["Blue"].b;
                        break;

                    case "Objective":
                        Debug.Log("Instanciating an objective...");
                        objectInstance = Instantiate(ObjectivePrefab, ItemsContainer.transform);
                        objectInstance.tag = "objective";
                        Objective objective = objectInstance.GetComponentInChildren<Objective>();
                        objective.Red = jsonEntity["Red"].b;
                        objective.Green = jsonEntity["Green"].b;
                        objective.Blue = jsonEntity["Blue"].b;
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
                        filterMirror.Red = jsonEntity["Red"].b;
                        filterMirror.Green = jsonEntity["Green"].b;
                        filterMirror.Blue = jsonEntity["Blue"].b;
                        break;

                    case "Light Source":
                        Debug.Log("Instanciating a light source...");
                        objectInstance = Instantiate(LightSourcePrefab, ItemsContainer.transform);
                        Laser laser = objectInstance.GetComponentInChildren<Laser>();
                        foreach (var jsonRay in jsonEntity["Rays"].list)
                        {
                            RayColor rayColor =
                                new RayColor(jsonRay["Red"].b, jsonRay["Green"].b, jsonRay["Blue"].b, 0.9f);
                            RaySource raySource =
                                new RaySource((Direction) jsonRay["Direction"].i, jsonRay["Enabled"].b, rayColor);
                            laser.AddSource(raySource);
                        }
                        break;
                    case "Obstacle":
                        Debug.Log("Instanciating an obstacle...");
                        objectInstance = Instantiate(ObstaclePrefab, ItemsContainer.transform);
                        break;
                    default:
                        Debug.LogError(string.Format("Object of type {0} is not supported.", jsonEntity["Type"].str));
                        break;
                }

                if (objectInstance == null) continue;
                
                Vector3 pos = new Vector3(jsonEntity["X"].n, jsonEntity["Y"].n, 0);
                objectInstance.transform.position = pos;
                DragAndDrop dragAndDrop = objectInstance.GetComponentInChildren<DragAndDrop>();
                dragAndDrop.IsDraggable = jsonEntity["Draggable"].b;
                BoardManager.AddItemPosition(pos);
            }
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
		Objective[] objectives = GetAllObjectives ();
		if (objectives.All(objective => objective.Completed))
		{
			Debug.LogWarning("Congratulations! All Objectives have been completed!");
            WinLevel();
        }
    }

    public void WinLevel()
    {
        WinScreen.SetActive(true);
    }
}