using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Utilities;
using UnityEditorInternal;
using UnityEngine.Experimental.UIElements;

public class Spawn : MonoBehaviour {

    public TDManager TDManager;
	public GameObject MonsterPrefab;
	public float SpawnInterval;
    private int _enemiesLeft;

    public void SetUp(TDManager TDManager, float interval) {
        this.TDManager = TDManager;
		this.SpawnInterval = interval;
        // Might add more parameters
	}

    public void StartWave (List<RayColor> wave) {
        StartCoroutine (SpawnEnemies (wave));
	}

    private IEnumerator SpawnEnemies(List<RayColor> wave) {
        _enemiesLeft = wave.Count;
        while (_enemiesLeft > 0) {
            Instantiate (MonsterPrefab, transform.position, Quaternion.identity);
            _enemiesLeft--;
            yield return new WaitForSeconds (SpawnInterval);
        }
        StopAllCoroutines ();
    }

}
