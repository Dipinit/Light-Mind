using System.IO;
using UnityEngine;

namespace Menu
{
    public class LevelSelection : MonoBehaviour
    {
        public GameObject LevelButton;
        public LevelItem[] LevelButtons;


	// Use this for initialization
	public void Start () {
		var levelReached = PlayerPrefs.GetInt("levelReached", 1);
		BetterStreamingAssets.Initialize();
		var paths = BetterStreamingAssets.GetFiles("/", "*TD.json", SearchOption.TopDirectoryOnly);
		var i = 0;
		LevelButtons = new LevelItem[paths.Length];
		foreach (var path in paths)
		{
			var fname = path.Split('_')[0];
			var level = Instantiate(LevelButton, transform);
			var levelItem = level.GetComponent<LevelItem>();
			levelItem.SetName(i.ToString());
			levelItem.SetFilename(fname);
			if (i <= levelReached) 
			{
				levelItem.SetInteractable(true);
			}

			LevelButtons[i] = levelItem;

			i++;
            }
        }
    }
}