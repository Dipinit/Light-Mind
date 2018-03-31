using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    public class EditGameData : EditorWindow
    {
        public List<JSONObject> Levels = new List<JSONObject>();

        private const string GameDataFolder = ".";
        private string _gameDataPath;

        [MenuItem("Light Mind/Game Data Viewer")]
        public static void Init()
        {
            GetWindow(typeof(EditGameData));
        }

        void OnGUI()
        {
            GUILayout.Label("Level information", EditorStyles.boldLabel);
            
            if (Levels.Count > 0)
            {
                if (GUILayout.Button("Save data"))
                {
                    SaveGameData();
                }
            }

            if (GUILayout.Button("Load data"))
            {
                LoadGameData();
            }
        }

        private void LoadGameData()
        {
            _gameDataPath = Path.Combine(Application.streamingAssetsPath, GameDataFolder);
            Debug.Log(string.Format("Loading data from {0}...", _gameDataPath));

            var directoryInfo = new DirectoryInfo(_gameDataPath);
            var levelDataFiles = directoryInfo.GetFiles()
                .Where(fi => fi.Name.StartsWith("level") && fi.Name.EndsWith("_TD.json")).ToArray();
            
            Debug.Log(levelDataFiles);
            
            foreach (var levelDataFile in levelDataFiles)
            {
                Debug.Log(string.Format("Loading {0}", levelDataFile.FullName));
                var levelDataAsJson = File.ReadAllText(levelDataFile.FullName);
                var loadedLevel = JsonUtility.FromJson<JSONObject>(levelDataAsJson);
                Levels.Add(loadedLevel);
            }
        }

        private void SaveGameData()
        {
            foreach (var level in Levels)
            {
                var levelDataAsJson = level.Print();
                var levelDataFilePath =
                    Path.Combine(_gameDataPath, string.Format("level{0}_TD.json", level["Number"].i));
                File.WriteAllText(levelDataFilePath, levelDataAsJson);
            }
        }
    }
}