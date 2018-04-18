using Assets.Scripts.Utilities;
using Behaviors;
using UnityEngine;

namespace Items
{
    public class TurretBehaviour : ItemBase
    {
        [Header("General")] public bool Enabled;
        public float Range = 15f;

        [Header("Use Bullets (default)")] public GameObject BulletPrefab;
        public float FireRate = 1f;
        private float _fireCountdown;

        [Header("Use Laser")] public bool UseLaser;
        public int LaserDamageBasePerSecond = 30;
        public LineRenderer LineRenderer;
        public ParticleSystem ImpactEffect;
        public Light ImpactLight;

        private const float LaserDamageUpdateRate = 0.2f;

        [Header("Unity Setup Fields")] public Transform PartToRotate;
        public float RotateSpeed = 10f;

        public Transform FirePoint;

        private Transform _currentTarget;

        // Use this for initialization
        private void Start()
        {
            InvokeRepeating("UpdateTarget", 0f, 0.5f);
            InvokeRepeating("ShootLaser", 0f, 1f * LaserDamageUpdateRate);
        }

        // Update is called once per frame
        public override void Update()
        {
            base.Update();

            if (_currentTarget == null || !Enabled)
            {
                if (!UseLaser || !LineRenderer.enabled) return;
                LineRenderer.enabled = false;
                ImpactEffect.Stop();
                ImpactLight.enabled = false;

                return;
            }

            LockOnTarget();

            if (UseLaser)
            {
                ToggleLaser();
            }
            else
            {
                // Shoot at target
                if (_fireCountdown <= 0f)
                {
                    ShootBullet();
                    _fireCountdown = 1f / FireRate;
                }

                _fireCountdown -= Time.deltaTime;
            }
        }

        private void ToggleLaser()
        {
            if (!LineRenderer.enabled)
            {
                LineRenderer.enabled = true;
                ImpactEffect.Play();
                ImpactLight.enabled = true;
            }

            LineRenderer.SetPosition(0, FirePoint.position);
            LineRenderer.SetPosition(1, _currentTarget.position);

            var direction = FirePoint.position - _currentTarget.position;

            ImpactEffect.transform.position = _currentTarget.position + direction.normalized * 0.5f;
            ImpactEffect.transform.rotation = Quaternion.LookRotation(direction);
        }

        private void ShootLaser()
        {
            if (!Enabled || !UseLaser || _currentTarget == null) return;
            
            var damage = 0f;

            var enemy = _currentTarget.GetComponent<EnemyBehaviour>();
            Debug.Log(enemy);
            if (enemy == null) return;

            if (enemy.Color.B != Color.B) damage += LaserDamageBasePerSecond;
            if (enemy.Color.G != Color.G) damage += LaserDamageBasePerSecond;
            if (enemy.Color.R != Color.R) damage += LaserDamageBasePerSecond;

            damage *= LaserDamageUpdateRate;

            var damageInt = (int) damage;
                        
            enemy.Life -= damageInt;
            
            Debug.Log(string.Format("{0} laser hit a {1} enemy causing {2} damage ({3} remaining)",
                Color.GetName(),
                enemy.Color.GetName(),
                damage,
                enemy.Life
            ));
        }

        private void LockOnTarget()
        {
            // Lock on nearest enemy
            var directionToTarget = _currentTarget.position - transform.position;
            var lookRotation = Quaternion.LookRotation(directionToTarget);
            var rotation = Quaternion.Lerp(PartToRotate.rotation, lookRotation, Time.deltaTime * RotateSpeed)
                .eulerAngles;

            PartToRotate.rotation = Quaternion.Euler(0f, rotation.y, 0f);
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

        private void ShootBullet()
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
            CalculateColor();
        }

        public override void HitExit(Ray ray)
        {
            base.HitExit(ray);
            CalculateColor();
        }

        private void CalculateColor()
        {
            if (ReceveidRays.Count > 0)
            {
                RayColor color = ReceveidRays[0].Color;
                for (int i = 1; i < ReceveidRays.Count; i++)
                {
                    color = RayColor.Add(color, ReceveidRays[i].Color);
                }

                Color = color;
                Enabled = true;
            }
            else
            {
                Color = RayColor.NONE;
                Enabled = false;
            }

            if (Color != RayColor.NONE && UseLaser && LineRenderer != null)
            {
                SetLineRendererColor(Color);
            }
        }
        
                
        private void SetLineRendererColor(RayColor color)
        {
            // Set a gradient with the same color at the beginning and the end (we have to use a Gradient...)
            var gradient = new Gradient();
            gradient.SetKeys(
                new[]
                {
                    new GradientColorKey(color.GetColor(), 0.0f), new GradientColorKey(color.GetColor(), 1.0f)
                },
                new[]
                {
                    new GradientAlphaKey(color.Alpha, 0.0f), new GradientAlphaKey(color.Alpha, 1.0f)
                }
            );
            
            Debug.Log(gradient);

            // Apply the gradient
            LineRenderer.colorGradient = gradient;
        }
    }
}