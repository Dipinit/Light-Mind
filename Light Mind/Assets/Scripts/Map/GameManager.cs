using System;
using System.IO;
using Assets.Scripts.Objects;
using Assets.Scripts.Utilities;
using Assets.Scripts.Utilities.Json;
using UnityEngine;
using UnityEngine.SceneManagement;

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
                string dataAsJson = File.ReadAllText(filePath); 
                Debug.Log(dataAsJson);
            
                Entity[] loadedEntities = JsonHelper.FromJson<Entity>(dataAsJson);
                foreach (var entity in loadedEntities)
                {
                    Debug.Log(entity);
                    GameObject gameObject = null;
                    switch (entity.Type)
                    {
                        case "Mirror":
                            Debug.Log("Instanciating a mirror...");
                            gameObject = Instantiate(MirrorPrefab, this.transform);
                            Mirror mirror = gameObject.GetComponentInChildren<Mirror>();
                            JsonMirror jsonMirror = JsonUtility.FromJson<JsonMirror>(entity.Data);
                            mirror.Orientation = jsonMirror.Orientation;
                            break;
                            
                        case "Filter":
                            Debug.Log("Instanciating a filter...");
                            gameObject = Instantiate(FilterPrefab, transform);
                            Filter filter = gameObject.GetComponentInChildren<Filter>();
                            JsonFilter jsonFilter = JsonUtility.FromJson<JsonFilter>(entity.Data);
                            filter.Red = jsonFilter.Red;
                            filter.Green = jsonFilter.Green;
                            filter.Blue = jsonFilter.Blue;
                            break;
                            
                        case "Objective":
                            Debug.Log("Instanciating an objective...");
                            gameObject = Instantiate(ObjectivePrefab, transform);
                            Objective objective = gameObject.GetComponentInChildren<Objective>();
                            JsonObjective jsonObjective = JsonUtility.FromJson<JsonObjective>(entity.Data);
                            objective.Red = jsonObjective.Red;
                            objective.Green = jsonObjective.Green;
                            objective.Blue = jsonObjective.Blue;
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
                            JsonFilterMirror jsonFilterMirror = JsonUtility.FromJson<JsonFilterMirror>(entity.Data);
                            filterMirror.Orientation = jsonFilterMirror.Orientation;
                            filterMirror.Red = jsonFilterMirror.Red;
                            filterMirror.Green = jsonFilterMirror.Green;
                            filterMirror.Blue = jsonFilterMirror.Blue;
                            break;
                        
                        
                        case "Light Source":
                            break;
                            Debug.Log("Instanciating a light source...");
                            gameObject = Instantiate(LightSourcePrefab, transform);
                            Laser laser = gameObject.GetComponentInChildren<Laser>();
                            Debug.Log(entity.Data);
                            JsonRaySource[] jsonRaySources = JsonHelper.FromJson<JsonRaySource>(entity.Data);
                            if (jsonRaySources == null || jsonRaySources.Length != 8)
                                throw new Exception("mdr le caca");
                            foreach (var jsonRaySource in jsonRaySources)
                            {
                                laser.AddSource(new RaySource(
                                    jsonRaySource.Direction, 
                                    jsonRaySource.Enabled, 
                                    new RayColor(
                                        jsonRaySource.Red, 
                                        jsonRaySource.Green, 
                                        jsonRaySource.Blue, 
                                        0.9f)));
                            }
                            break;
                    }

                    DragAndDrop dragAndDrop = gameObject.GetComponentInChildren<DragAndDrop>();
                    Vector3 pos = new Vector3(entity.X, entity.Y, 0);
                    gameObject.transform.position = dragAndDrop.SnapToGrid(pos);
                }

            }

        }
    }
}