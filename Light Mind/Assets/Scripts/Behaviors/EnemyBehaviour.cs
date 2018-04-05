using Assets.Scripts.Utilities;
using UnityEngine;

namespace Behaviors
{
	public class EnemyBehaviour : MonoBehaviour
	{
		public delegate void EnemyDelegate(GameObject enemy);

		public EnemyDelegate enemyDelegate;

		[SerializeField] private float _speed = 1.2f;

		// Enemy movement AI
		private Transform _targetWaypoint;
		private int _currentWaypointIndex;

		public RayColor Color;

		private void Start()
		{
			_targetWaypoint = GameManager.Instance.BoardManager.Waypoints[_currentWaypointIndex];
		}

		private void Update()
		{
			// Set enemy color
			GetComponent<Renderer>().material.color = Color.GetColor();
			
			if (_targetWaypoint == null) return;

			var directionToNextWaypoint = _targetWaypoint.position - transform.position;
			transform.Translate(directionToNextWaypoint.normalized * _speed * Time.deltaTime, Space.World);

			if (Vector3.Distance(transform.position, _targetWaypoint.position) <= 0.2f)
			{
				GetNextWaypoint();
			}
		}

		private void GetNextWaypoint()
		{
			if (_currentWaypointIndex >= GameManager.Instance.BoardManager.Waypoints.Count - 1)
			{
				// Enemy has reached end point, let's reduce life
				Destroy(gameObject);
				GameManager.Instance.TdManager.DecreaseLives();
				return;
			}
            
			_currentWaypointIndex++;
			_targetWaypoint = GameManager.Instance.BoardManager.Waypoints[_currentWaypointIndex];
		}

		private void OnDestroy()
		{
			if (enemyDelegate != null)
			{
				enemyDelegate(gameObject);
			}
		}
	}
}