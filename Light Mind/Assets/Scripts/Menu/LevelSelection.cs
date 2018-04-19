using System.IO;
using System.Runtime.InteropServices;
using UnityEngine;
using System;

namespace Menu
{
	public class LevelSelection : MonoBehaviour
	{
		public GameObject LevelButton;
		public LevelItem[] LevelButtons;


		// Use this for initialization
		public void Start()
		{
			var levelReached = Int32.Parse(PlayerPrefs.GetString ("currentLevel").Substring (5));

			BetterStreamingAssets.Initialize();
			var paths = BetterStreamingAssets.GetFiles("/", "*TD.json", SearchOption.TopDirectoryOnly);
			JSONObject customLevels = LevelManager.GetCustomLevels();
			var i = 0;
			LevelButtons = new LevelItem[paths.Length + customLevels.Count + 1];
			foreach (var path in paths)
			{
				var fname = path.Split('_')[0];
				var level = Instantiate(LevelButton, transform);
				var levelItem = level.GetComponent<LevelItem>();
				levelItem.SetName(i.ToString());
				levelItem.SetFilename(fname);
				levelItem.IsCustom = false;
				if (i <= levelReached)
				{
					levelItem.SetInteractable(true);
				}

				LevelButtons[i] = levelItem;

				i++;
			}

			for (int j = 0; j < customLevels.Count; j++)
			{
				JSONObject jsonLevel = customLevels[j];
				var level = Instantiate(LevelButton, transform);
				var levelItem = level.GetComponent<LevelItem>();
				levelItem.IsCustom = true;
				levelItem.SetName("C");
				levelItem.SetFilename(jsonLevel["Name"].str);
				levelItem.SetInteractable(true);

				LevelButtons[i] = levelItem;

				i++;
			}
		}
	}
}
