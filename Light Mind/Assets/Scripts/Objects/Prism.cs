using Assets.Scripts.Utilities;
using UnityEngine;

namespace Assets.Scripts.Objects
{
    public class Prism : MonoBehaviour, IHitObject
    {
        private RayEmitter _blueRayEmitter;
        private RayEmitter _redRayEmitter;
        private RayEmitter _greenRayEmitter;

        private Direction _hitDirection;
        private RayColor _rayColor;

        // Use this for initialization
        private void Start()
        {
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

        private void RenderLasers()
        {
            RenderBlueLaser();
            RenderGreenLaser();
            RenderRedLaser();
        }

        private void RenderBlueLaser()
        {
            var direction = DirectionUtility.GetDirectionAsVector3(_hitDirection);
            direction = Quaternion.AngleAxis(90, Vector3.forward) * direction;

            _blueRayEmitter.Emit(DirectionUtility.ToDirection(direction));
        }

        private void RenderRedLaser()
        {
            var direction = DirectionUtility.GetDirectionAsVector3(_hitDirection);
            direction = Quaternion.AngleAxis(0, Vector3.forward) * direction;

            _redRayEmitter.Emit(DirectionUtility.ToDirection(direction));
        }

        private void RenderGreenLaser()
        {
            var direction = DirectionUtility.GetDirectionAsVector3(_hitDirection);
            direction = Quaternion.AngleAxis(-90, Vector3.forward) * direction;

            _greenRayEmitter.Emit(DirectionUtility.ToDirection(direction));
        }

        public void HitEnter(Direction hitDirection, RayColor rayColor)
        {
            _hitDirection = hitDirection;
            _blueRayEmitter.Enable(rayColor.B);
            _greenRayEmitter.Enable(rayColor.G);
            _redRayEmitter.Enable(rayColor.R);
        }

        public void HitExit()
        {
            _blueRayEmitter.Enable(false);
            _greenRayEmitter.Enable(false);
            _redRayEmitter.Enable(false);
        }
    }
}