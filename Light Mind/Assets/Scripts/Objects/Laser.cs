using UnityEngine;
using Utilities;

namespace Objects
{
    public class Laser : MonoBehaviour
    {
        private LineRenderer _lineRenderer;
        public bool On;
        public Direction EmitDirection;
        private HitObject _hitObject;
        private Vector3 _direction;

        // Use this for initialization
        void Start()
        {
            _lineRenderer = GetComponent<LineRenderer>();
            _lineRenderer.enabled = On;

  
        }

        // Update is called once per frame
        void Update()
        {
            _direction = DirectionUtility.getDirectionAsVector3(EmitDirection);
            
            if (On)
            {
                if (!_lineRenderer.enabled)
                {
                    _lineRenderer.enabled = true;
                }
                
                // Set initial position
                _lineRenderer.SetPosition(0, transform.position);

                // Check if laser hit an object
                RaycastHit hit;
                if (Physics.Raycast(transform.position, _direction, out hit))
                {
                    if (hit.collider)
                    {
                        _lineRenderer.SetPosition(1, hit.point);
                        HitObject obj = hit.transform.gameObject.GetComponent<HitObject>();
                        if (obj != null)
                        {
                            if (_hitObject != null && obj != _hitObject)
                            {
                                _hitObject.hitExit();
                            }
                            _hitObject = obj;
                            _hitObject.hitEnter(_direction);
                        }
                    }
                }
                else
                {
                    _lineRenderer.SetPosition(1, _direction * 5000);
                    if (_hitObject != null)
                    {
                        _hitObject.hitExit();
                    }
                }
            }
            else
            {
                if (_lineRenderer.enabled)
                {
                    _lineRenderer.enabled = false;
                }
            }
        }
    }
}