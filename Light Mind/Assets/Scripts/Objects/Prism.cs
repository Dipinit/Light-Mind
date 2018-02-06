using Assets.Scripts.Utilities;
using UnityEngine;

namespace Assets.Scripts.Objects
{
    public class Prism : RaySensitive
    {
        // Use this for initialization
        public override void Start()
        {
            base.Start();
        }

        // Update is called once per frame
        public override void Update()
        {
            base.Update();
        }

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
            if (ray.Color.R)
            {
                EmitNewRay(GetRedRayDirection(ray.Direction), new RayColor(true, false, false, 0.9f), ray);
            }
            
            if (ray.Color.G)
            {
                EmitNewRay(GetGreenRayDirection(ray.Direction), new RayColor(false, true, false, 0.9f), ray);
            }
            
            if (ray.Color.B)
            {
                EmitNewRay(GetBlueRayDirection(ray.Direction), new RayColor(false, false, true, 0.9f), ray);
            }
        }
        
        // Launched when a ray hits the filter
        public override void HitEnter(Ray ray)
        {
            base.HitEnter(ray);
            Debug.Log("Hit enter in Prism");
            HandleReceivedRay(ray);
        }
    }
}