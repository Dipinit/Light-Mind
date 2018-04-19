using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.InteropServices;
using System;

public class FunfactSelection : MonoBehaviour {
	public GameObject FunfactButton;
	public GameObject ScrollFunFact;
	public GameObject ScrollFunFactDetail;

	// Use this for initialization
	void Start () {
		var levelReached = Int32.Parse(PlayerPrefs.GetString ("currentLevel").Substring (5));
		DirectoryInfo dir = new DirectoryInfo("Assets/StreamingAssets/");
		FileInfo[] info = dir.GetFiles("*TD.json");
		var i = 0;
		foreach (FileInfo f in info)
		{
				string fname = f.Name.Split('_')[0];
				var funfact = Instantiate(FunfactButton, transform);
				var _funfactItem = funfact.GetComponent<FunfactItem>();
				_funfactItem.SetName(i.ToString());
				_funfactItem.SetFilename(fname);
				Debug.Log(levelReached);

				if(i <= levelReached) {
					_funfactItem.SetInteractable(true);
				}
				_funfactItem.ScrollFunFact = ScrollFunFact;
				_funfactItem.ScrollFunFactDetail = ScrollFunFactDetail;
				i++;
		}
	}
}
