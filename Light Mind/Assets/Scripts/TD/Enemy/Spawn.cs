using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Utilities;
using UnityEditorInternal;
using UnityEngine.Experimental.UIElements;
using System.IO;

public class Spawn : MonoBehaviour {

    public TDManager TDManager;
	public GameObject MonsterPrefab;
	public float SpawnInterval;
    private int _enemiesSpawned = 0;
    private List<Vector3> _paths;

    public void SetUp(TDManager TDManager, float interval, List<Vector3> paths) {
        this.TDManager = TDManager;
		this.SpawnInterval = interval;
        this._paths = paths;
        this._enemiesSpawned = 0;
        // Might add more parameters
	}

    public void StartWave (List<RayColor> wave) {
        StartCoroutine (SpawnEnemies (wave));
	}

    void Update() {
        if (TDManager != null && _enemiesSpawned > 0 && TDManager.GameState == TDManager.STATE.PLAYING) {
            GameObject[] enemies = GameObject.FindGameObjectsWithTag ("enemy");
            if (enemies.Length == 0) {
                TDManager.CallNextPhase ();
            }
        }
    }

    private IEnumerator SpawnEnemies(List<RayColor> wave) {
        _enemiesSpawned = 0;
        Debug.Log ("Wave Count: " + wave.Count);
        Vector3 enemyPos = transform.position;
        enemyPos.z = -1;
        while (_enemiesSpawned < wave.Count) {
            GameObject enemyGo = Instantiate (MonsterPrefab, enemyPos, Quaternion.identity);
            Debug.Log ("Spawned");
            Enemy enemy = (Enemy)enemyGo.GetComponent (typeof(Enemy));
            enemy.Init (_paths, wave[_enemiesSpawned]);
            _enemiesSpawned++;
            yield return new WaitForSeconds (SpawnInterval);
        }
        StopAllCoroutines ();
    }

}
