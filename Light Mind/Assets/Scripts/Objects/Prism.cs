using Assets.Scripts.Utilities;
using UnityEngine;

namespace Assets.Scripts.Objects
{
    public class Prism : MonoBehaviour, IHitObject
    {
        // The emitter of the left blue ray
        private RayEmitter _blueRayEmitter;
        
        // The emitter of the front red ray
        private RayEmitter _redRayEmitter;
        
        // The emitter of the green right ray
        private RayEmitter _greenRayEmitter;

        // The direction of the 
        private Direction _hitDirection;
        private RayColor _rayColor;

        // Use this for initialization
        private void Start()
        {
            // Initiate emitters and disable them
            _blueRayEmitter = new RayEmitter(transform.Find("Blue").GetComponent<LineRenderer>(),
                new RayColor(false, false, true, 0.9f));
            _blueRayEmitter.Enable(false);

            _greenRayEmitter = new RayEmitter(transform.Find("Green").GetComponent<LineRenderer>(),
                new RayColor(false, true, false, 0.9f));
            _greenRayEmitter.Enable(false);

            _redRayEmitter = new RayEmitter(transform.Find("Red").GetComponent<LineRenderer>(),
                new RayColor(true, false, false, 0.9f));
            _redRayEmitter.Enable(false);
        }

        // Update is called once per frame
        private void Update()
        {
            RenderLasers();
        }

        // Draw blue, green and red lasers
        private void RenderLasers()
        {
            RenderBlueLaser();
            RenderGreenLaser();
            RenderRedLaser();
        }

        // Draw blue laser
        private void RenderBlueLaser()
        {
            var direction = DirectionUtility.GetDirectionAsVector3(_hitDirection);
            direction = Quaternion.AngleAxis(90, Vector3.forward) * direction;

            _blueRayEmitter.Emit(DirectionUtility.ToDirection(direction));
        }

        // Draw red laser
        private void RenderRedLaser()
        {
            var direction = DirectionUtility.GetDirectionAsVector3(_hitDirection);
            direction = Quaternion.AngleAxis(0, Vector3.forward) * direction;

            _redRayEmitter.Emit(DirectionUtility.ToDirection(direction));
        }

        // Draw green laser
        private void RenderGreenLaser()
        {
            var direction = DirectionUtility.GetDirectionAsVector3(_hitDirection);
            direction = Quaternion.AngleAxis(-90, Vector3.forward) * direction;

            _greenRayEmitter.Emit(DirectionUtility.ToDirection(direction));
        }

        // Launched when a ray hits the prism
        public void HitEnter(Direction hitDirection, RayColor rayColor)
        {
            // Store the hit direction
            _hitDirection = hitDirection;
            
            // Enable the blue emitter if the hitting ray contains blue color
            _blueRayEmitter.Enable(rayColor.B);
            
            // Enable the green emitter if the hitting ray contains green color
            _greenRayEmitter.Enable(rayColor.G);
            
            // Enable the red emitter if the hitting ray contains red color
            _redRayEmitter.Enable(rayColor.R);
        }

        // Launched when a ray stops hitting the prism
        public void HitExit()
        {
            // Disable emitters
            _blueRayEmitter.Enable(false);
            _greenRayEmitter.Enable(false);
            _redRayEmitter.Enable(false);
        }
    }
}