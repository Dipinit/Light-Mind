using System.Runtime.Serialization.Formatters;
using Assets.Scripts.Utilities;
using UnityEngine;

namespace Assets.Scripts.Objects
{
    public class RayEmitter
    {
        // The line renderer used to draw rays
        private readonly LineRenderer _lineRenderer;
        
        // The object that was hit by the ray
        private GameObject _hitGameObject;
        
        // The current ray color
        private RayColor _rayColor;
        
        // The current emitter state
        private bool _enabled;

        // Public constructor with default white ray color
        public RayEmitter(LineRenderer lineRenderer)
        {
            _lineRenderer = lineRenderer;
            _rayColor = new RayColor(true, true, true, 1.0f);
            _enabled = true;
        }

        // Public constructor with specified ray color
        public RayEmitter(LineRenderer lineRenderer, RayColor rayColor)
        {
            _lineRenderer = lineRenderer;
            _rayColor = rayColor;
            _enabled = true;
        }

        // Draw a line in a direction
        public void Emit(Direction direction)
        {
            // If the emitter is enabled and the ray color is not black...
            if (_enabled && (_rayColor.R || _rayColor.B || _rayColor.G))
            {
                // Update the line renderer color
                SetRayColor();

                // Enable the line renderer if it's not
                if (!_lineRenderer.enabled)
                {
                    _lineRenderer.enabled = true;
                }

                // Set line initial position
                _lineRenderer.SetPosition(0, _lineRenderer.transform.position);
                
                // Set the line renderer position count to two positions (only one segment)
                _lineRenderer.positionCount = 2;

                // Check if the ray hits an object in the input direction
                RaycastHit hit;
                if (Physics.Raycast(_lineRenderer.transform.position, DirectionUtility.GetDirectionAsVector3(direction),
                    out hit))
                {
                    // If the ray hits an object with a collider
                    if (hit.collider)
                    {
                        // Set the end position to the object that was hit position
                        _lineRenderer.SetPosition(1,
                            hit.point + 0.5F * DirectionUtility.GetDirectionAsVector3(direction));
                        
                        // Check if the object that was hit is a HitObject
                        var obj = hit.transform.gameObject;
                        var hitObject = obj.GetComponent<IHitObject>();
                        if (hitObject == null) return;
                        
                        // If the object that was hit is the same as the current object, return
                        if (_hitGameObject != null && (_hitGameObject == null || obj == _hitGameObject)) return;
                        
                        // If the object that was hit is not the same as the current object, call HitExit to the current
                        // object
                        if (_hitGameObject != null)
                            _hitGameObject.GetComponent<IHitObject>().HitExit();

                        // Store the new HitObject
                        _hitGameObject = obj;
                        
                        // Log a new Hit evenement
                        Debug.Log(string.Format("Ray hit {0} {1} with direction {2} and color {3}",
                            _hitGameObject.transform.parent.gameObject, _hitGameObject.GetInstanceID(), direction,
                            _rayColor.GetColor()));
                        
                        // Call HitEnter on the new HitObject
                        hitObject.HitEnter(direction, _rayColor);
                    }
                    // If the ray hits an object with not collider and object and a current HitObject is set
                    else if (_hitGameObject != null)
                    {
                        // Log the end of a Hit event
                        Debug.Log(string.Format("Ray stopped hitting {0} {1} with direction {2}",
                            _hitGameObject.transform.parent.gameObject, _hitGameObject.GetInstanceID(), direction));
                        
                        // Call HitExit on the current HitObject
                        _hitGameObject.GetComponent<IHitObject>().HitExit();
                        
                        // Set the current HitObject to null
                        _hitGameObject = null;
                    }
                }
                // If the ray hits nothing
                else
                {
                    // Draw a line in the input direction to the infinite
                    _lineRenderer.SetPosition(1, DirectionUtility.GetDirectionAsVector3(direction) * 5000);
                    
                    // If no current HitOject is set, return
                    if (_hitGameObject == null) return;
                    
                    // Else, Log the end of a Hit event
                    Debug.Log(string.Format("Ray stopped hitting {0} {1} with direction {2}",
                        _hitGameObject.transform.parent.gameObject, _hitGameObject.GetInstanceID(), direction));
                    
                    // Call HitExit on the current HitObject
                    _hitGameObject.GetComponent<IHitObject>().HitExit();
                    
                    // Set the current HitObject to null
                    _hitGameObject = null;
                }
            }
            // If the emitter is not enabled or is black
            else
            {
                // Disabe the line renderer if it is not already
                if (_lineRenderer.enabled)
                {
                    _lineRenderer.enabled = false;
                }
            }
        }

        // Enable or disable the emitter
        public void Enable(bool boolean)
        {
            // If enabling
            if (boolean)
            {
                _enabled = true;
            }
            // If disable
            else
            {
                _enabled = false;
                // If no current HitOject is set, return
                if (_hitGameObject == null) return;
                
                // Else, Log the end of a Hit event
                Debug.Log(string.Format("Ray stopped hitting {0} {1}", _hitGameObject.transform.parent.gameObject,
                    _hitGameObject.GetInstanceID()));
                
                // Call HitExit on the current HitObject
                _hitGameObject.GetComponent<IHitObject>().HitExit();
                
                // Set the current HitObject to null
                _hitGameObject = null;
            }
        }

        // Set the line renderer color
        private void SetRayColor()
        {
            // Set a gradient with the same color at the beginning and the end (we have to use a Gradient...)
            var gradient = new Gradient();
            gradient.SetKeys(
                new[]
                {
                    new GradientColorKey(_rayColor.GetColor(), 0.0f), new GradientColorKey(_rayColor.GetColor(), 1.0f)
                },
                new[]
                    {new GradientAlphaKey(_rayColor.Alpha, 0.0f), new GradientAlphaKey(_rayColor.Alpha, 1.0f)}
            );
            
            // Apply the gradient
            _lineRenderer.colorGradient = gradient;
        }

        // Public function then change the ray color
        public void SetRayColor(RayColor rayColor)
        {
            // If the input color is the same as the current, do nottinh
            if (rayColor == _rayColor) return;
            
            // Store the new value
            _rayColor = rayColor;
            
            // If no current HitOject is set, return
            if (_hitGameObject == null) return;
            
            // Else, Log the end of a Hit evenement, because the change of the color is considered as a different hit
            // event
            Debug.Log(string.Format("Ray stopped hitting {0} {1}", _hitGameObject.transform.parent.gameObject,
                _hitGameObject.GetInstanceID()));
            
            // Call HitExit on the current HitObject
            _hitGameObject.GetComponent<IHitObject>().HitExit();
            
            // Set the current HitObject to null
            _hitGameObject = null;
        }
    }
}