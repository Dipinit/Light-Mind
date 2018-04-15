using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager {
    const string CustomLevelsKey = "custom-levels";
    
    public static JSONObject GetCustomLevels()
    {
        if (!PlayerPrefs.HasKey("custom-levels"))
            ResetLevels();
        
        string customLevel = PlayerPrefs.GetString(CustomLevelsKey);
        return new JSONObject(customLevel);
    }

    public static void SaveCustomLevels(JSONObject levels)
    {
        PlayerPrefs.SetString(CustomLevelsKey, levels.ToString());
        PlayerPrefs.Save();
    }

    public static void SaveLevel(JSONObject level, string levelName)
    {
        JSONObject levels = GetCustomLevels();
        levels.AddField(levelName, level);
        SaveCustomLevels(levels);
    }

    public static JSONObject LoadLevel(string levelName)
    {
        JSONObject levels = GetCustomLevels();
        JSONObject level = levels[levelName];
        return level;
    }

    public static void DeleteLevel(string levelName)
    {
        JSONObject levels = GetCustomLevels();
        if (levels.HasField(levelName))
        {
            levels.RemoveField(levelName);
            SaveCustomLevels(levels);
        }
    }

    public static void ResetLevels()
    {
        JSONObject jsonObject = new JSONObject();
        PlayerPrefs.SetString(CustomLevelsKey, jsonObject.ToString());
        PlayerPrefs.Save();
    }
}
