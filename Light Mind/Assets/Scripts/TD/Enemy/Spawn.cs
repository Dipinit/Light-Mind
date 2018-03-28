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
    private int _enemiesLeft;
    private List<Vector3> _paths;

    public void SetUp(TDManager TDManager, float interval, List<Vector3> paths) {
        this.TDManager = TDManager;
		this.SpawnInterval = interval;
        this._paths = paths;
        // Might add more parameters
	}

    public void StartWave (List<RayColor> wave) {
        StartCoroutine (SpawnEnemies (wave));
	}

    private IEnumerator SpawnEnemies(List<RayColor> wave) {
        _enemiesLeft = wave.Count;
        Vector3 enemyPos = transform.position;
        enemyPos.z = -1;
        while (_enemiesLeft > 0) {
            GameObject enemyGo = Instantiate (MonsterPrefab, enemyPos, Quaternion.identity);
            Enemy enemy = (Enemy)enemyGo.GetComponent (typeof(Enemy));
            enemy.Init (_paths);
            _enemiesLeft--;
            yield return new WaitForSeconds (SpawnInterval);
        }
        StopAllCoroutines ();
    }

}
