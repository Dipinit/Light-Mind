using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelection : MonoBehaviour {
	public void Select(string levelName) {
		SceneManager.LoadScene(levelName);
	}
}
