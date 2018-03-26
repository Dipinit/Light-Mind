using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn : MonoBehaviour {

	public GameObject monsterPrefab;
	public float interval = 3;
	public int wave = 1;
	public int enemiesLeft;

	// Later on add ArrayOfEnemyTypes to see which enemies to spawn

	void SetUp(float interval, int wave, int enemiesLeft) {
		this.interval = interval;
		this.wave = wave;
		this.enemiesLeft = enemiesLeft;
	}

	public void StartWave () {
		while (enemiesLeft > 0) {
			Invoke ("SpawnNext", interval);
		}
	}

	void SpawnNext() {
		Instantiate (monsterPrefab, transform.position, Quaternion.identity);
		enemiesLeft--;
	}

}
