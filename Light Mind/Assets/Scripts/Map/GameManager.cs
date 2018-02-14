using System;
using System.IO;
using Assets.Scripts.Objects;
using Assets.Scripts.UI;
using Assets.Scripts.Utilities;
using UnityEngine;

namespace Assets.Scripts.Map
{
    public class GameManager : MonoBehaviour
    {
        public GameObject GridPrefab;
        public GameObject MirrorPrefab;
        public GameObject FilterPrefab;
        public GameObject ObjectivePrefab;
        public GameObject FilterMirrorPrefab;
        public GameObject LightSourcePrefab;
        public GameObject PrismPrefab;
        public GameObject MirrorInventoryItemPrefab;
        public GameObject FilterMirrorInventoryItemPrefab;
        public GameObject PrismInventoryItemPrefab;
        public GameObject FilterInventoryItemPrefab;
        public GameObject Inventory;

        private void Start()
        {
            string currentLevel = PlayerPrefs.GetString("currentLevel");
            if (!String.IsNullOrEmpty(currentLevel))
            {
                LoadLevel(PlayerPrefs.GetString("currentLevel"));
            }
        }
        
        public void LoadLevel(string level)
        {
            Instantiate(GridPrefab);
            string filePath = Path.Combine(Application.streamingAssetsPath, level + ".json");
            if(File.Exists(filePath))
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
                    GameObject gameObject = null;
                    switch (jsonEntity["Type"].str)
                    {
                        case "Mirror":
                            Debug.Log("Instanciating a mirror...");
                            gameObject = Instantiate(MirrorPrefab, this.transform);
                            Mirror mirror = gameObject.GetComponentInChildren<Mirror>();
                            mirror.Orientation = (Direction) jsonEntity["Orientation"].i;
                            break;
                            
                        case "Filter":
                            Debug.Log("Instanciating a filter...");
                            gameObject = Instantiate(FilterPrefab, transform);
                            Filter filter = gameObject.GetComponentInChildren<Filter>();
                            filter.Red = jsonEntity["Red"].b;
                            filter.Green = jsonEntity["Green"].b;
                            filter.Blue = jsonEntity["Blue"].b;
                            break;
                            
                        case "Objective":
                            Debug.Log("Instanciating an objective...");
                            gameObject = Instantiate(ObjectivePrefab, transform);
                            Objective objective = gameObject.GetComponentInChildren<Objective>();
                            objective.Red = jsonEntity["Red"].b;
                            objective.Green = jsonEntity["Green"].b;
                            objective.Blue = jsonEntity["Blue"].b;
                            break;
                            
                        case "Prism":
                            Debug.Log("Instanciating a prism...");
                            gameObject = Instantiate(PrismPrefab, transform);
                            Prism prism = gameObject.GetComponentInChildren<Prism>();
                            break;
                        
                        case "Filter Mirror":
                            Debug.Log("Instanciating a filter mirror...");
                            gameObject = Instantiate(FilterMirrorPrefab, transform);
                            FilterMirror filterMirror = gameObject.GetComponentInChildren<FilterMirror>();
                            filterMirror.Orientation = (Direction) jsonEntity["Orientation"].i;
                            filterMirror.Red = jsonEntity["Red"].b;
                            filterMirror.Green = jsonEntity["Green"].b;
                            filterMirror.Blue = jsonEntity["Blue"].b;
                            break;
                        
                        case "Light Source":
                            Debug.Log("Instanciating a light source...");
                            gameObject = Instantiate(LightSourcePrefab, transform);
                            Laser laser = gameObject.GetComponentInChildren<Laser>();
                            foreach (var jsonRay in jsonEntity["Rays"].list)
                            {
                                RayColor rayColor = 
                                    new RayColor(jsonRay["Red"].b, jsonRay["Green"].b, jsonRay["Blue"].b, 0.9f);
                                RaySource raySource =
                                    new RaySource((Direction) jsonRay["Direction"].i, jsonRay["Enabled"].b, rayColor);
                                laser.AddSource(raySource);
                            }
                            break;
                    }

                    if (gameObject != null)
                    {
                        DragAndDrop dragAndDrop = gameObject.GetComponentInChildren<DragAndDrop>();
                        Vector3 pos = new Vector3(jsonEntity["X"].n, jsonEntity["Y"].n, 0);
                        gameObject.transform.position = dragAndDrop.SnapToGrid(pos);
                    }
                }

            }

        }
    }
}