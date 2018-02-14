using Assets.Scripts.Utilities;
using UnityEngine;

namespace Items
{
    [RequireComponent(typeof(AudioSource))]
    [RequireComponent(typeof(ParticleSystem))]
    public class Prism : ItemBase
    {
        private AudioSource[] _Prism_Sound;
        private ParticleSystem _Prism_Particules;

        private Direction GetBlueRayDirection(Direction direction)
        {
            var blueRayDirection = DirectionUtility.GetDirectionAsVector3(direction);
            blueRayDirection = Quaternion.AngleAxis(90, Vector3.forward) * blueRayDirection;
            return DirectionUtility.ToDirection(blueRayDirection);
        }
        
        private Direction GetGreenRayDirection(Direction direction)
        {
            var greenRayDirection = DirectionUtility.GetDirectionAsVector3(direction);
            greenRayDirection = Quaternion.AngleAxis(-90, Vector3.forward) * greenRayDirection;
            return DirectionUtility.ToDirection(greenRayDirection);
        }
        
        private Direction GetRedRayDirection(Direction direction)
        {
            var redRayDirection = DirectionUtility.GetDirectionAsVector3(direction);
            redRayDirection = Quaternion.AngleAxis(0, Vector3.forward) * redRayDirection;
            return DirectionUtility.ToDirection(redRayDirection);
        }
        
        public override void HandleReceivedRay(Ray ray)
        {
            _Prism_Particules = GetComponent<ParticleSystem>();
            if (ray.Color.R)
            {
                EmitNewRay(GetRedRayDirection(ray.Direction), new RayColor(true, false, false, 0.9f), ray);
                _Prism_Particules.Play();
            }
            
            if (ray.Color.G)
            {
                EmitNewRay(GetGreenRayDirection(ray.Direction), new RayColor(false, true, false, 0.9f), ray);
                _Prism_Particules.Play();
            }
            
            if (ray.Color.B)
            {
                EmitNewRay(GetBlueRayDirection(ray.Direction), new RayColor(false, false, true, 0.9f), ray);
                _Prism_Particules.Play();
            }
        }
        
        // Launched when a ray hits the filter
        public override void HitEnter(Ray ray)
        {
            _Prism_Sound = GetComponents<AudioSource>();
            _Prism_Sound[1].Play();
            base.HitEnter(ray);
            HandleReceivedRay(ray);
        }
    }
}