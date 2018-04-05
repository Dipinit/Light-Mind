using UnityEngine;

namespace Behaviors
{
    public class Bullet : MonoBehaviour
    {
        [Header("Fire Properties")] public float Speed = 70f;
        public float ExplosionRadius;

        [Header("FX")] public GameObject ImpactEffect;

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
            }

            var directionToTarget = _target.position - transform.position;
            var distanceThisFrame = Speed * Time.deltaTime;

            if (directionToTarget.magnitude <= distanceThisFrame)
            {
                HitTarget();
                return;
            }

            transform.Translate(directionToTarget.normalized * distanceThisFrame, Space.World);
            transform.LookAt(_target);
        }

        private void HitTarget()
        {
            var effectInstance = Instantiate(ImpactEffect, transform.position, transform.rotation);
            Destroy(effectInstance, 5f);

            if (ExplosionRadius > 0f)
            {
                Explode();
            }
            else
            {
                Damage(_target);
            }

            Destroy(gameObject);
        }

        private void Explode()
        {
            var hitColliders = Physics.OverlapSphere(transform.position, ExplosionRadius);
            foreach (var hitCollider in hitColliders)
            {
                if (hitCollider.CompareTag("enemy"))
                {
                    Damage(hitCollider.transform);
                }
            }
        }

        private void Damage(Component enemy)
        {
            Destroy(enemy.gameObject);
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, ExplosionRadius);
        }
    }
}