using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class LevelSelection : MonoBehaviour {
	public GameObject LevelButton;

	// Use this for initialization
	void Start () {
		DirectoryInfo dir = new DirectoryInfo("Assets/StreamingAssets/");
		FileInfo[] info = dir.GetFiles("*TD.json");
		var i = 0;
		foreach (FileInfo f in info)
		{
				string fname = f.Name.Split('_')[0];
				var level = Instantiate(LevelButton, transform);
				var _levelItem = level.GetComponent<LevelItem>();
				_levelItem.SetName(i.ToString());
				_levelItem.SetFilename(fname);
				i++;
		}
	}
}
