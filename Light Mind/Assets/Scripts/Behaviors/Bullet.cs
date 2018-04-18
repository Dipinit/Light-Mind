using Assets.Scripts.Utilities;
using UnityEngine;

namespace Behaviors
{
    public class Bullet : MonoBehaviour
    {
        [Header("Fire Properties")] public float Speed = 70f;
        public float ExplosionRadius;

        public int DamageBase = 20;
        public RayColor Color = RayColor.WHITE;

        [Header("FX")] public GameObject ImpactEffect;

        private Transform _target;

        public AudioClip shootSound;


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
            var enemyBehaviour = enemy.GetComponent<EnemyBehaviour>();
            if (enemyBehaviour == null) return;

            var damage = CalculateDamage(enemyBehaviour.Color);
            enemyBehaviour.Life -= damage;
            Debug.Log(string.Format("{0} bullet hit a {1} enemy causing {2} damage ({3} remaining)",
                Color.GetName(),
                enemyBehaviour.Color.GetName(),
                damage,
                enemyBehaviour.Life
            ));
        }

        private int CalculateDamage(RayColor enemyColor)
        {
            var damage = 0;

            if (enemyColor.B != Color.B) damage += DamageBase;
            if (enemyColor.G != Color.G) damage += DamageBase;
            if (enemyColor.R != Color.R) damage += DamageBase;

            return damage;
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = UnityEngine.Color.red;
            Gizmos.DrawWireSphere(transform.position, ExplosionRadius);
        }
    }
}