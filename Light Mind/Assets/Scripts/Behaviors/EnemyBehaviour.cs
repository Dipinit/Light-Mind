using Assets.Scripts.Utilities;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

namespace Behaviors
{
    public class EnemyBehaviour : MonoBehaviour
    {
        public delegate void EnemyDelegate(GameObject enemy);

        public EnemyDelegate EnemyDestroyCallback;
        public float StartLife = 100;
        public float Life;
        public float Speed = 10;

        // Enemy movement AI
        [Header("Navigation")] [SerializeField]
        private NavMeshAgent _navigationAgent;

        private Transform _end;

        public RayColor Color;

        [Header("Unity Stuff")]
        public Image HealthBar;

        private void Start()
        {
            // Set enemy color
            GetComponent<Renderer>().material.color = Color.GetColor();
            Life = StartLife;
            // Navigate enemy to the end
            _end = GameObject.FindGameObjectWithTag("Finish").transform;
            _navigationAgent.SetDestination(_end.position);
        }

        public void UpdateNavigationAgent() {
            _navigationAgent.speed = Speed;
        }

        private void Update()
        {
            HealthBar.fillAmount = Life / StartLife;
            if (Life <= 0)
            {
                Destroy(this);
            }
        }

        private void OnDestroy()
        {
            if (EnemyDestroyCallback != null)
            {
                EnemyDestroyCallback(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
}