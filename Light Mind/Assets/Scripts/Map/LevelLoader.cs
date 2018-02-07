using System;
using System.IO;
using Assets.Scripts.Objects;
using Assets.Scripts.Utilities;
using Assets.Scripts.Utilities.Json;
using UnityEditor.PackageManager;
using UnityEngine;

namespace Assets.Scripts.Map
{
    public class LevelLoader : MonoBehaviour
    {
        public GameObject MirrorPrefab;
        public GameObject FilterPrefab;
        public GameObject ObjectivePrefab;
        public GameObject FilterMirrorPrefab;
        public GameObject LightSourcePrefab;
        public GameObject PrismPrefab;
        public string Level;

        private void Start()
        {
           
            string filePath = Path.Combine(Application.streamingAssetsPath, Level + ".json");
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
                            gameObject = Instantiate(MirrorPrefab);
                            Mirror mirror = gameObject.GetComponentInChildren<Mirror>();
                            JsonMirror jsonMirror = JsonUtility.FromJson<JsonMirror>(entity.Data);
                            mirror.Orientation = jsonMirror.Orientation;
                            break;
                            
                        case "Filter":
                            gameObject = Instantiate(FilterPrefab);
                            Filter filter = gameObject.GetComponentInChildren<Filter>();
                            JsonFilter jsonFilter = JsonUtility.FromJson<JsonFilter>(entity.Data);
                            filter.Red = jsonFilter.Red;
                            filter.Green = jsonFilter.Green;
                            filter.Blue = jsonFilter.Blue;
                            break;
                            
                        case "Objective":
                            gameObject = Instantiate(ObjectivePrefab);
                            Objective objective = gameObject.GetComponentInChildren<Objective>();
                            JsonObjective jsonObjective = JsonUtility.FromJson<JsonObjective>(entity.Data);
                            objective.Red = jsonObjective.Red;
                            objective.Green = jsonObjective.Green;
                            objective.Blue = jsonObjective.Blue;
                            break;
                            
                        case "Prism":
                            gameObject = Instantiate(PrismPrefab);
                            Prism prism = gameObject.GetComponentInChildren<Prism>();
                            break;
                        
                        case "Filter Mirror":
                            gameObject = Instantiate(FilterMirrorPrefab);
                            FilterMirror filterMirror = gameObject.GetComponentInChildren<FilterMirror>();
                            JsonFilterMirror jsonFilterMirror = JsonUtility.FromJson<JsonFilterMirror>(entity.Data);
                            filterMirror.Orientation = jsonFilterMirror.Orientation;
                            filterMirror.Red = jsonFilterMirror.Red;
                            filterMirror.Green = jsonFilterMirror.Green;
                            filterMirror.Blue = jsonFilterMirror.Blue;
                            break;
                        
                        
                        case "Light Source":
                            gameObject = Instantiate(LightSourcePrefab);
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