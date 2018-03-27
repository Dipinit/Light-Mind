using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Utilities;
using UnityEditorInternal;

public class Spawn : MonoBehaviour {

    public TDManager TDManager;
	public GameObject MonsterPrefab;
	public float SpawnInterval;
    private int _enemiesLeft;

    void SetUp(TDManager TDManager, float interval) {
        this.TDManager = TDManager;
		this.SpawnInterval = interval;
        // Might add more parameters
	}

    public void StartWave (List<RayColor> wave) {
        _enemiesLeft = wave.Count;
		while (_enemiesLeft > 0) {
			Invoke ("SpawnNext", SpawnInterval);
            if (TDManager.GameState != TDManager.STATE.PLAYING) {
                _enemiesLeft = 0;
                Debug.Log ("Player lost, stopping enemy spawns.");
            }
		}
	}

	void SpawnNext() {
		Instantiate (MonsterPrefab, transform.position, Quaternion.identity);
		_enemiesLeft--;
	}

}
