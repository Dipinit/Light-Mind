using Assets.Scripts.Utilities;
using UnityEngine;
using UnityEngine.AI;

namespace Behaviors
{
    public class EnemyBehaviour : MonoBehaviour
    {
        public delegate void EnemyDelegate(GameObject enemy);

        public EnemyDelegate EnemyDestroyCallback;
        public int Life = 100;

        // Enemy movement AI
        [Header("Navigation")] [SerializeField]
        private NavMeshAgent _navigationAgent;

        private Transform _end;

        public RayColor Color;

        private void Start()
        {
            // Set enemy color
            GetComponent<Renderer>().material.color = Color.GetColor();

            // Navigate enemy to the end
            _end = GameObject.FindGameObjectWithTag("Finish").transform;
            _navigationAgent.SetDestination(_end.position);
        }

        private void Update()
        {
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