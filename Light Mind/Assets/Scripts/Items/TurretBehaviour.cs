using Items;
using UnityEngine;
using Ray = Items.Ray;

namespace Behaviors
{
    public class TurretBehaviour : ItemBase
    {
        [Header("Attributes")]
        public float Range = 15f;
        public float FireRate = 1f;
        private float _fireCountdown = 0f;
        
        [Header("Unity Setup Fields")]
        public Transform PartToRotate;
        public float RotateSpeed = 10f;

        public GameObject BulletPrefab;
        public Transform FirePoint;
        
        private Transform _currentTarget;

        // Use this for initialization
        private void Start()
        {
            InvokeRepeating("UpdateTarget", 0f, 0.5f);
        }

        // Update is called once per frame
        private void Update()
        {
            if (_currentTarget == null) return;

            // Lock on nearest enemy
            var directionToTarget = _currentTarget.position - transform.position;
            var lookRotation = Quaternion.LookRotation(directionToTarget);
            var rotation = Quaternion.Lerp(PartToRotate.rotation, lookRotation, Time.deltaTime * RotateSpeed)
                .eulerAngles;

            PartToRotate.rotation = Quaternion.Euler(0f, rotation.y, 0f);
            
            // Shoot at target
            if (_fireCountdown <= 0f)
            {
                Shoot();
                _fireCountdown = 1f / FireRate;
            }

            _fireCountdown -= Time.deltaTime;
        }

        private void UpdateTarget()
        {
            var shortestDistance = Mathf.Infinity;
            GameObject nearestEnemy = null;

            foreach (var enemy in GameObject.FindGameObjectsWithTag("enemy"))
            {
                var distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
                if (!(distanceToEnemy < shortestDistance)) continue;

                shortestDistance = distanceToEnemy;
                nearestEnemy = enemy;
            }

            if (nearestEnemy != null && shortestDistance <= Range)
            {
                _currentTarget = nearestEnemy.transform;
            }
            else
            {
                _currentTarget = null;
            }
        }

        private void Shoot()
        {
            var bullet = Instantiate(BulletPrefab, FirePoint.position, FirePoint.rotation);
            Bullet bulletComponent = bullet.GetComponent<Bullet>();
            bulletComponent.DamageBase = 40;
            bulletComponent.Color = Color;
            bulletComponent.Seek(_currentTarget);
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = UnityEngine.Color.red;
            Gizmos.DrawWireSphere(transform.position, Range);
        }
        
        // Launched when a ray hits the mirror
        public override void HitEnter(Ray ray)
        {
            base.HitEnter(ray);
            HandleReceivedRay(ray);
        }
        
        public override void HandleReceivedRay(Ray ray)
        {
            SetColor(ray.Color);
        }
    }
}