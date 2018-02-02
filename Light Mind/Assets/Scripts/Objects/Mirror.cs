using System;
using Assets.Scripts.Utilities;
using UnityEngine;

namespace Assets.Scripts.Objects
{
    public class Mirror : MonoBehaviour, IHitObject
    {
        // The Orientable component. Used to change the mirror direction 
        private Orientable _orientable;
        
        // The direction of the reflected ray
        private Direction _reflectionDirection;
        
        // The direction of the hitting ray
        private Direction _hitDirection;
        
        // Is the mirror reflecting a light (= is a ray hitting the mirror on a valid face) ?
        private bool _isReflecting;
        
        // Is the mirror currently hit by a ray (on any face) ?
        private bool _isHit;
        
        // The emitter of the reflected ray
        private RayEmitter _rayEmitter;

        // Use this for initialization
        private void Start()
        {
            _rayEmitter = new RayEmitter(GetComponent<LineRenderer>());
            _rayEmitter.Enable(false);

            _isReflecting = false;
            _isHit = false;
            _hitDirection = Direction.East;
            _reflectionDirection = Direction.East;

            _orientable = GetComponent<Orientable>();
        }

        // Update is called once per frame
        private void Update()
        {
            // Update the reflection state
            UpdateReflection();
            
            // Enabled the emitter based on the reflection state
            _rayEmitter.Enable(_isReflecting);
            
            // Draw the ray based on the reflection state
            _rayEmitter.Emit(_reflectionDirection);
        }

        private void UpdateReflection()
        {
            // If the mirror is not currently hit...
            if (!_isHit)
            {
                // ... it is also not reflecting
                _isReflecting = false;
            }
            // If the mirror is currently hit ...
            else
            {
                // ... Update of the reflection state
                switch (_orientable.Orientation)
                {
                    case Direction.East:
                        // If the hitting ray is on a valid face...
                        if (_hitDirection == Direction.SouthWest || _hitDirection == Direction.NorthWest)
                        {
                            // ... setting the reflected ray direction based on the hitting ray direction
                            if (_hitDirection == Direction.SouthWest)
                                _reflectionDirection = Direction.SouthEast;
                            if (_hitDirection == Direction.NorthWest)
                                _reflectionDirection = Direction.NorthEast;
                            _isReflecting = true;
                        }
                        // If the hitting ray is not on a valid face...
                        else
                        {
                            // ... the mirror is not reflecting
                            _isReflecting = false;
                        }

                        break;
                        
                    // etc for each direction (somewhat long but working and fast)
                    case Direction.NorthEast:
                        if (_hitDirection == Direction.West || _hitDirection == Direction.South)
                        {
                            if (_hitDirection == Direction.West)
                                _reflectionDirection = Direction.North;
                            if (_hitDirection == Direction.South)
                                _reflectionDirection = Direction.East;
                            _isReflecting = true;
                        }
                        else
                        {
                            _isReflecting = false;
                        }

                        break;
                    case Direction.North:
                        if (_hitDirection == Direction.SouthEast || _hitDirection == Direction.SouthWest)
                        {
                            if (_hitDirection == Direction.SouthEast)
                                _reflectionDirection = Direction.NorthEast;
                            if (_hitDirection == Direction.SouthWest)
                                _reflectionDirection = Direction.NorthWest;
                            _isReflecting = true;
                        }
                        else
                        {
                            _isReflecting = false;
                        }

                        break;
                    case Direction.NorthWest:
                        if (_hitDirection == Direction.East || _hitDirection == Direction.South)
                        {
                            if (_hitDirection == Direction.East)
                                _reflectionDirection = Direction.North;
                            if (_hitDirection == Direction.South)
                                _reflectionDirection = Direction.West;
                            _isReflecting = true;
                        }
                        else
                        {
                            _isReflecting = false;
                        }

                        break;
                    case Direction.West:
                        if (_hitDirection == Direction.NorthEast || _hitDirection == Direction.SouthEast)
                        {
                            if (_hitDirection == Direction.NorthEast)
                                _reflectionDirection = Direction.SouthEast;
                            if (_hitDirection == Direction.SouthEast)
                                _reflectionDirection = Direction.NorthEast;
                            _isReflecting = true;
                        }
                        else
                        {
                            _isReflecting = false;
                        }

                        break;
                    case Direction.SouthWest:
                        if (_hitDirection == Direction.East || _hitDirection == Direction.North)
                        {
                            if (_hitDirection == Direction.East)
                                _reflectionDirection = Direction.South;
                            if (_hitDirection == Direction.North)
                                _reflectionDirection = Direction.West;
                            _isReflecting = true;
                        }
                        else
                        {
                            _isReflecting = false;
                        }

                        break;
                    case Direction.South:
                        if (_hitDirection == Direction.NorthWest || _hitDirection == Direction.NorthEast)
                        {
                            if (_hitDirection == Direction.NorthWest)
                                _reflectionDirection = Direction.SouthWest;
                            if (_hitDirection == Direction.NorthEast)
                                _reflectionDirection = Direction.SouthEast;
                            _isReflecting = true;
                        }
                        else
                        {
                            _isReflecting = false;
                        }

                        break;
                    case Direction.SouthEast:
                        if (_hitDirection == Direction.West || _hitDirection == Direction.North)
                        {
                            if (_hitDirection == Direction.West)
                                _reflectionDirection = Direction.South;
                            if (_hitDirection == Direction.North)
                                _reflectionDirection = Direction.East;
                            _isReflecting = true;
                        }
                        else
                        {
                            _isReflecting = false;
                        }

                        break;
                    default:
                        // Throw an exception if the input direction is wrong
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        // Launched when a ray hits the mirror
        public void HitEnter(Direction hitDirection, RayColor rayColor)
        {
            // Storing the hitting ray direction
            _hitDirection = hitDirection;
            
            // Enable the hitting state
            _isHit = true;
            
            // Apply the ray color to the emitter
            _rayEmitter.SetRayColor(rayColor);
        }

        // Launched when a ray stops hitting the filter
        public void HitExit()
        {
            // Disable the hitting state
            _isHit = false;
            
            // Disable the emitter
            _rayEmitter.Enable(false);
        }
    }
}