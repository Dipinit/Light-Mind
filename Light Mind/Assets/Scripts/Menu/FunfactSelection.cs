using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class FunfactSelection : MonoBehaviour {
	public GameObject FunfactButton;
	public GameObject ScrollFunFact;
	public GameObject ScrollFunFactDetail;
	public FunfactItem[] FunfactButtons;

	// Use this for initialization
	void Start () {
		int levelReached = PlayerPrefs.GetInt("levelReached", 1);
		DirectoryInfo dir = new DirectoryInfo("Assets/StreamingAssets/");
		FileInfo[] info = dir.GetFiles("*TD.json");
		var i = 0;
		FunfactButtons = new FunfactItem[info.Length];
		foreach (FileInfo f in info)
		{
				string fname = f.Name.Split('_')[0];
				var funfact = Instantiate(FunfactButton, transform);
				var _funfactItem = funfact.GetComponent<FunfactItem>();
				_funfactItem.SetName(i.ToString());
				_funfactItem.SetFilename(fname);
				if(i < levelReached) {
					_funfactItem.SetInteractable(true);
				}
				_funfactItem.ScrollFunFact = ScrollFunFact;
				_funfactItem.ScrollFunFactDetail = ScrollFunFactDetail;
				FunfactButtons[i] = _funfactItem;

				i++;
		}
	}
}
