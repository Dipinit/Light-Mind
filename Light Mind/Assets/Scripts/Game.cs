using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Objects;

public class Game : MonoBehaviour
{
	// Temp variable to avoid log spamming
	public bool LevelCompleted;

	private Objective[] _objectives;

	// Should initialize or count other gameObjects to limit available gameObjects for Player


	// Use this for initialization
	void Start ()
	{
		LevelCompleted = false;
		getAllObjectives ();
	}

	// Update is called once per frame
	void Update ()
	{
		if (isLevelCompleted ()) {
			if (LevelCompleted)
				return;
			Debug.Log ("Level Completed!");
			LevelCompleted = true;
		}
	}

	// Find the parent GameObject of all Quad Objects and get all objectives references
	private void getAllObjectives ()
	{
		_objectives = GameObject.FindGameObjectWithTag ("objective")
							    .GetComponentsInChildren<Objective> ();
	}

	// Checks for a win condition (all objectives completed)
	private bool isLevelCompleted ()
	{
		foreach (var objective in _objectives) {
			if (!objective.Completed) {
				return false;
			}
		}
		return true;
	}
}
