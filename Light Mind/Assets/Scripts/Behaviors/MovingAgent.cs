using UnityEngine;
using UnityEngine.AI;

namespace Behaviors
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class MovingAgent : MonoBehaviour
    {
        private void Start()
        {
            var agent = GetComponent<NavMeshAgent>();
            var endpoint = GameObject.Find("END");
            agent.destination = endpoint.transform.position;
        }
    }
}