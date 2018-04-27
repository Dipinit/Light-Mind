using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager {
    const string CustomLevelsKey = "custom-levels";
   
    /// <summary>
    /// Load the custom levels to be selected..
    /// </summary>
    /// <returns></returns>
    public static JSONObject GetCustomLevels()
    {
        if (!PlayerPrefs.HasKey("custom-levels"))
            ResetLevels();
        
        string customLevel = PlayerPrefs.GetString(CustomLevelsKey);
        return new JSONObject(customLevel);
    }

    /// <summary>
    /// Save a custom level.
    /// </summary>
    /// <param name="levels"></param>
    public static void SaveCustomLevels(JSONObject levels)
    {
        PlayerPrefs.SetString(CustomLevelsKey, levels.ToString());
        PlayerPrefs.Save();
    }

    /// <summary>
    /// Save a level.
    /// </summary>
    /// <param name="level"></param>
    /// <param name="levelName"></param>
    public static void SaveLevel(JSONObject level, string levelName)
    {
        JSONObject levels = GetCustomLevels();
        levels.AddField(levelName, level);
        SaveCustomLevels(levels);
    }

    /// <summary>
    /// Load a level.
    /// </summary>
    /// <param name="levelName">Represents the name of a level.</param>
    /// <returns></returns>
    public static JSONObject LoadLevel(string levelName)
    {
        JSONObject levels = GetCustomLevels();
        JSONObject level = levels[levelName];
        return level;
    }

    /// <summary>
    /// Delete a selected level.
    /// </summary>
    /// <param name="levelName"></param>
    public static void DeleteLevel(string levelName)
    {
        JSONObject levels = GetCustomLevels();
        if (levels.HasField(levelName))
        {
            levels.RemoveField(levelName);
            SaveCustomLevels(levels);
        }
    }

    /// <summary>
    /// Resets all the levels.
    /// </summary>
    public static void ResetLevels()
    {
        JSONObject jsonObject = new JSONObject();
        PlayerPrefs.SetString(CustomLevelsKey, jsonObject.ToString());
        PlayerPrefs.Save();
    }
}
