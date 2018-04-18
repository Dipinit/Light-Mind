using System.IO;
using UnityEngine;

namespace Menu
{
    public class LevelSelection : MonoBehaviour
    {
        public GameObject LevelButton;
        public LevelItem[] LevelButtons;

        // Use this for initialization
        private void Start()
        {
            var levelReached = PlayerPrefs.GetInt("levelReached", 1);
            var dir = new DirectoryInfo("Assets/StreamingAssets/");
            var info = dir.GetFiles("*TD.json");
            var i = 0;
            LevelButtons = new LevelItem[info.Length];
            foreach (var f in info)
            {
                var fname = f.Name.Split('_')[0];
                var level = Instantiate(LevelButton, transform);
                var levelItem = level.GetComponent<LevelItem>();
                levelItem.SetName(i.ToString());
                levelItem.SetFilename(fname);
                if (i < levelReached)
                {
                    levelItem.SetInteractable(true);
                }

                LevelButtons[i] = levelItem;

                i++;
            }
        }
    }
}