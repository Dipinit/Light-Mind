using UnityEngine;
using Utilities;

namespace Objects
{
    public class Laser : MonoBehaviour
    {
        public bool On;
        public bool Red;
        public bool Green;
        public bool Blue;
        public float Alpha;

        private RayColor _rayColor;
        private LineRenderer _lineRenderer;
        private HitObject _hitObject;
        private Orientable _orientable;

        // Use this for initialization
        void Start()
        {
            _lineRenderer = GetComponent<LineRenderer>();
            _lineRenderer.enabled = On;

            _orientable = GetComponent<Orientable>();
        }

        // Update is called once per frame
        void Update()
        {            
            if (On)
            {
                SetRayColor();
                
                if (!_lineRenderer.enabled)
                {
                    _lineRenderer.enabled = true;
                }
                
                // Set initial position
                _lineRenderer.SetPosition(0, transform.position);
                _lineRenderer.positionCount = 2;

                // Check if laser hit an object
                RaycastHit hit;
               
                if (Physics.Raycast(transform.position,  _orientable.GetVector3(), out hit))
                {
                    if (hit.collider)
                    {
                        _lineRenderer.SetPosition(1, hit.point + 0.5F * _orientable.GetVector3());
                        HitObject obj = hit.transform.gameObject.GetComponent<HitObject>();
                        if (obj != null)
                        {
                            if (_hitObject != null && obj != _hitObject)
                            {
                                _hitObject.HitExit();
                            }
                            _hitObject = obj;
                            _hitObject.HitEnter(_orientable.Orientation, _rayColor);
                        }
                    }
                }
                else
                {
                    _lineRenderer.SetPosition(1, _orientable.GetVector3() * 5000);
                    if (_hitObject != null)
                    {
                        _hitObject.HitExit();
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

        void SetRayColor()
        {               
            _rayColor = new RayColor(Red, Green, Blue, Alpha);
            
            // A simple 2 color gradient with a fixed alpha of 1.0f.
            Gradient gradient = new Gradient();
            gradient.SetKeys(
                new GradientColorKey[] { new GradientColorKey(_rayColor.GetColor(), 0.0f), new GradientColorKey(_rayColor.GetColor(), 1.0f) },
                new GradientAlphaKey[] { new GradientAlphaKey(_rayColor.alpha, 0.0f), new GradientAlphaKey(_rayColor.alpha, 1.0f) }
            );
            _lineRenderer.colorGradient = gradient;
        }
    }
}