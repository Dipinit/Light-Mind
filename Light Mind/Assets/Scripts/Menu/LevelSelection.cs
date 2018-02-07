using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Map;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelection : MonoBehaviour {
	public void Select(string levelName)
	{
		PlayerPrefs.SetString("currentLevel", levelName);
		SceneManager.LoadScene("Game");
		/*
		Scene scene = SceneManager.GetSceneByName("Game");
		foreach (var gameObject in scene.GetRootGameObjects())
		{
			GameManager gameManager = gameObject.GetComponent<GameManager>();
			if (gameManager != null)
			{
				gameManager.LoadLevel(levelName);
				break;
			}
		}
		*/
	}
}
