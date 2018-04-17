using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FunFactDetail : MonoBehaviour {

	void Update () {
		var _text = this.GetComponentInChildren<Text>();
		_text.text = 		PlayerPrefs.GetString("FunFact");
	}
}
