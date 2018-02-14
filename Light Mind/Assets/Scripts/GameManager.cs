using System;
using System.IO;
using Assets.Scripts.Utilities;
using Assets.Scripts.Utilities.Json;
using Behaviors;
using Items;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public BoardManager BoardManager;
    public GameObject ItemsContainer;

    public GameObject MirrorPrefab;
    public GameObject FilterPrefab;
    public GameObject ObjectivePrefab;
    public GameObject FilterMirrorPrefab;
    public GameObject LightSourcePrefab;
    public GameObject PrismPrefab;

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
            ItemsContainer = new GameObject();
            ItemsContainer.name = "Items";
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
        }
    }

    public void LoadLevel(string level)
    {
        // Instantiate(GridPrefab);
        string filePath = Path.Combine(Application.streamingAssetsPath, string.Format("{0}.json", level));
        if (File.Exists(filePath))
        {
            // Read the json from the file into a string
            string dataAsJson = File.ReadAllText(filePath);
            Debug.Log(dataAsJson);

            Entity[] loadedEntities = JsonHelper.FromJson<Entity>(dataAsJson);
            foreach (var entity in loadedEntities)
            {
                Debug.Log(entity);
                GameObject objectInstance = null;
                switch (entity.Type)
                {
                    case "Mirror":
                        Debug.Log("Instanciating a mirror...");
                        objectInstance = Instantiate(MirrorPrefab, ItemsContainer.transform);
                        Mirror mirror = objectInstance.GetComponentInChildren<Mirror>();
                        JsonMirror jsonMirror = JsonUtility.FromJson<JsonMirror>(entity.Data);
                        mirror.Orientation = jsonMirror.Orientation;
                        break;

                    case "Filter":
                        Debug.Log("Instanciating a filter...");
                        objectInstance = Instantiate(FilterPrefab, ItemsContainer.transform);
                        Filter filter = objectInstance.GetComponentInChildren<Filter>();
                        JsonFilter jsonFilter = JsonUtility.FromJson<JsonFilter>(entity.Data);
                        filter.Red = jsonFilter.Red;
                        filter.Green = jsonFilter.Green;
                        filter.Blue = jsonFilter.Blue;
                        break;

                    case "Objective":
                        Debug.Log("Instanciating an objective...");
                        objectInstance = Instantiate(ObjectivePrefab, ItemsContainer.transform);
                        Objective objective = objectInstance.GetComponentInChildren<Objective>();
                        JsonObjective jsonObjective = JsonUtility.FromJson<JsonObjective>(entity.Data);
                        objective.Red = jsonObjective.Red;
                        objective.Green = jsonObjective.Green;
                        objective.Blue = jsonObjective.Blue;
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
                        JsonFilterMirror jsonFilterMirror = JsonUtility.FromJson<JsonFilterMirror>(entity.Data);
                        filterMirror.Orientation = jsonFilterMirror.Orientation;
                        filterMirror.Red = jsonFilterMirror.Red;
                        filterMirror.Green = jsonFilterMirror.Green;
                        filterMirror.Blue = jsonFilterMirror.Blue;
                        break;


                    case "Light Source":
                        Debug.Log("Instanciating a light source...");
                        objectInstance = Instantiate(LightSourcePrefab, ItemsContainer.transform);
                        Laser laser = objectInstance.GetComponentInChildren<Laser>();
                        JsonRaySource[] jsonRaySources = JsonHelper.FromJson<JsonRaySource>(entity.Data);
                        if (jsonRaySources == null || jsonRaySources.Length != 8)
                            throw new Exception("Wrong light source");
                        foreach (var jsonRaySource in jsonRaySources)
                        {
                            Debug.Log(jsonRaySource);
                            RayColor rayColor =
                                new RayColor(jsonRaySource.Red, jsonRaySource.Green, jsonRaySource.Blue, 0.9f);
                            RaySource raySource =
                                new RaySource(jsonRaySource.Direction, jsonRaySource.Enabled, rayColor);
                            laser.AddSource(raySource);
                        }

                        break;
                }

                Vector3 pos = new Vector3(entity.X, entity.Y, 0);
                objectInstance.transform.position = pos;
            }
        }
    }
}