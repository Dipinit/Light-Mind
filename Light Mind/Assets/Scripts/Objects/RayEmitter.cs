using Assets.Scripts.Utilities;
using UnityEngine;

namespace Assets.Scripts.Objects
{
    public class RayEmitter
    {
        private readonly LineRenderer _lineRenderer;
        private GameObject _hitGameObject;
        private RayColor _rayColor;
        private bool _enabled;

        public RayEmitter(LineRenderer lineRenderer)
        {
            _lineRenderer = lineRenderer;
            _rayColor = new RayColor(true, true, true, 1.0f);
            _enabled = true;
        }

        public RayEmitter(LineRenderer lineRenderer, RayColor rayColor)
        {
            _lineRenderer = lineRenderer;
            _rayColor = rayColor;
            _enabled = true;
        }

        public void Emit(Direction direction)
        {
            if (_enabled && (_rayColor.R || _rayColor.B || _rayColor.G))
            {
                SetRayColor();

                if (!_lineRenderer.enabled)
                {
                    _lineRenderer.enabled = true;
                }

                // Set initial position
                _lineRenderer.SetPosition(0, _lineRenderer.transform.position);
                _lineRenderer.positionCount = 2;

                // Check if laser hit an object
                RaycastHit hit;

                if (Physics.Raycast(_lineRenderer.transform.position, DirectionUtility.GetDirectionAsVector3(direction),
                    out hit))
                {
                    if (hit.collider)
                    {
                        _lineRenderer.SetPosition(1,
                            hit.point + 0.5F * DirectionUtility.GetDirectionAsVector3(direction));
                        var obj = hit.transform.gameObject;
                        var hitObject = obj.GetComponent<IHitObject>();
                        if (hitObject == null) return;
                        if (_hitGameObject != null && (_hitGameObject == null || obj == _hitGameObject)) return;
                        if (_hitGameObject != null)
                            _hitGameObject.GetComponent<IHitObject>().HitExit();

                        _hitGameObject = obj;
                        Debug.Log(string.Format("Ray hit {0} {1} with direction {2} and color {3}",
                            _hitGameObject.transform.parent.gameObject, _hitGameObject.GetInstanceID(), direction,
                            _rayColor.GetColor()));
                        hitObject.HitEnter(direction, _rayColor);
                    }
                    else if (_hitGameObject != null)
                    {
                        Debug.Log(string.Format("Ray stopped hitting {0} {1} with direction {2}",
                            _hitGameObject.transform.parent.gameObject, _hitGameObject.GetInstanceID(), direction));
                        _hitGameObject.GetComponent<IHitObject>().HitExit();
                        _hitGameObject = null;
                    }
                }
                else
                {
                    _lineRenderer.SetPosition(1, DirectionUtility.GetDirectionAsVector3(direction) * 5000);
                    if (_hitGameObject == null) return;
                    Debug.Log(string.Format("Ray stopped hitting {0} {1} with direction {2}",
                        _hitGameObject.transform.parent.gameObject, _hitGameObject.GetInstanceID(), direction));
                    _hitGameObject.GetComponent<IHitObject>().HitExit();
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

        public void Enable(bool boolean)
        {
            if (boolean)
            {
                _enabled = true;
            }
            else
            {
                _enabled = false;
                if (_hitGameObject == null) return;
                Debug.Log(string.Format("Ray stopped hitting {0} {1}", _hitGameObject.transform.parent.gameObject,
                    _hitGameObject.GetInstanceID()));
                _hitGameObject.GetComponent<IHitObject>().HitExit();
                _hitGameObject = null;
            }
        }

        private void SetRayColor()
        {
            var gradient = new Gradient();
            gradient.SetKeys(
                new[]
                {
                    new GradientColorKey(_rayColor.GetColor(), 0.0f), new GradientColorKey(_rayColor.GetColor(), 1.0f)
                },
                new[]
                    {new GradientAlphaKey(_rayColor.Alpha, 0.0f), new GradientAlphaKey(_rayColor.Alpha, 1.0f)}
            );
            _lineRenderer.colorGradient = gradient;
        }

        public void SetRayColor(RayColor rayColor)
        {
            if (rayColor == _rayColor) return;
            _rayColor = rayColor;
            if (_hitGameObject == null) return;
            Debug.Log(string.Format("Ray stopped hitting {0} {1}", _hitGameObject.transform.parent.gameObject,
                _hitGameObject.GetInstanceID()));
            _hitGameObject.GetComponent<IHitObject>().HitExit();
            _hitGameObject = null;
        }
    }
}