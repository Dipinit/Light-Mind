using UnityEngine;
using UnityEngineInternal;
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
        private GameObject _hitGameObject;
        private Orientable _orientable;

        // Use this for initialization
        void Start()
        {
            _lineRenderer = GetComponent<LineRenderer>();
            _lineRenderer.enabled = On;
            _lineRenderer.material = new Material(Shader.Find("Sprites/Default"));

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
                        GameObject obj = hit.transform.gameObject;
                        HitObject hitObject = obj.GetComponent<HitObject>();
                        if (hitObject != null)
                        {                        
                            if (_hitGameObject == null || (_hitGameObject != null && obj != _hitGameObject))
                            {
                                if (_hitGameObject != null)
                                    _hitGameObject.GetComponent<HitObject>().HitExit();

                                _hitGameObject = obj;
                                Debug.Log("Laser hit " + _hitGameObject.transform.parent.gameObject.ToString() + " " + _hitGameObject.GetInstanceID() + " with direction " + _orientable.Orientation.ToString() + " and color " + _rayColor.GetColor());
                                hitObject.HitEnter(_orientable.Orientation, _rayColor);
                            }
                        }
                    }
                }
                else
                {
                    _lineRenderer.SetPosition(1, _orientable.GetVector3() * 5000);
                    if (_hitGameObject != null)
                    {
                        _hitGameObject.GetComponent<HitObject>().HitExit();
                    }

                    _hitGameObject = null;
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