using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Utilities;
using Behaviors;
using UnityEngine;

namespace TD.Enemy
{
    public class Spawn : MonoBehaviour
    {
        public float SpawnInterval;
        private int _enemiesSpawned;

        public void StartWave(List<RayColor> wave)
        {
            StartCoroutine(SpawnEnemies(wave));
        }

        private void Update()
        {
            if (_enemiesSpawned <= 0 || GameManager.Instance.TdManager.GameState != TDManager.State.Playing) return;

            var enemies = GameObject.FindGameObjectsWithTag("enemy");
            if (enemies.Length == 0)
            {
                GameManager.Instance.TdManager.CallNextPhase();
            }
        }

        private IEnumerator SpawnEnemies(IList<RayColor> wave)
        {
            _enemiesSpawned = 0;
            Debug.Log(string.Format("Wave Count: {0}", wave.Count));
            var enemyPos = transform.position;
            enemyPos.z = -1;
            while (_enemiesSpawned < wave.Count)
            {
                var enemyGo = Instantiate(GameManager.Instance.EnemyPrefab, enemyPos, Quaternion.identity);
                Debug.Log("Spawned");
                enemyGo.GetComponent<EnemyBehaviour>().Color = wave[_enemiesSpawned];
                _enemiesSpawned++;
                yield return new WaitForSeconds(SpawnInterval);
            }

            StopAllCoroutines();
        }
    }
}