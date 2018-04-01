using UnityEngine;

namespace Behaviors
{
    public class Bullet : MonoBehaviour
    {
        public float Speed = 70f;
        public GameObject ImpactEffect;
        
        private Transform _target;

        public void Seek(Transform target)
        {
            _target = target;
        }

        private void Update()
        {
            if (_target == null)
            {
                Destroy(gameObject);
                return;
            };

            var directionToTarget = _target.position - transform.position;
            var distanceThisFrame = Speed * Time.deltaTime;

            if (directionToTarget.magnitude <= distanceThisFrame)
            {
                HitTarget();
                return;
            }
            
            transform.Translate(directionToTarget.normalized * distanceThisFrame, Space.World);
        }

        private void HitTarget()
        {
            var effectInstance = Instantiate(ImpactEffect, transform.position, transform.rotation);
            Destroy(effectInstance, 2f);
            
            Destroy(gameObject);
        }
    }
}