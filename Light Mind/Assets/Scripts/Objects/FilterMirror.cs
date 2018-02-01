using System;
using Assets.Scripts.Utilities;
using UnityEngine;

namespace Assets.Scripts.Objects
{
    public class FilterMirror : MonoBehaviour, IHitObject
    {
        //Global 
        private RayColor _hitColor;
        private RayColor _color;

        // Mirror
        private RayEmitter _mirrorRayEmitter;
        private Orientable _orientable;
        private Direction _reflectionDirection;
        private bool _isReflecting;
        private bool _isHit;

        // Filter
        private RayEmitter _filterRayEmitter;
        private Direction _hitDirection;
        private MeshRenderer _meshRenderer;

        public bool Red;
        public bool Green;
        public bool Blue;

        // Use this for initialization
        private void Start()
        {
            // Mirror
            _mirrorRayEmitter = new RayEmitter(transform.Find("Mirror").GetComponent<LineRenderer>());
            _mirrorRayEmitter.Enable(false);
            _isReflecting = false;
            _isHit = false;
            _hitDirection = Direction.East;
            _reflectionDirection = Direction.East;
            _orientable = GetComponent<Orientable>();

            // Filter
            _filterRayEmitter = new RayEmitter(transform.Find("Filter").GetComponent<LineRenderer>());
            _filterRayEmitter.Enable(false);
            _meshRenderer = GetComponent<MeshRenderer>();
            _hitColor = new RayColor(true, true, true, 0.9f);
            SetColor();
        }

        private void Update()
        {
            // Mirror
            UpdateReflection();
            _mirrorRayEmitter.Enable(_isReflecting);
            _mirrorRayEmitter.Emit(_reflectionDirection);

            // Filter
            if (_color.R != Red || _color.G != Green || _color.B != Blue)
            {
                SetColor();
            }

            _filterRayEmitter.Emit(_hitDirection);
        }

        // Mirror
        private void UpdateReflection()
        {
            if (!_isHit)
            {
                _isReflecting = false;
            }
            else
            {
                switch (_orientable.Orientation)
                {
                    case Direction.East:
                        if (_hitDirection == Direction.SouthWest || _hitDirection == Direction.NorthWest)
                        {
                            if (_hitDirection == Direction.SouthWest)
                                _reflectionDirection = Direction.SouthEast;
                            if (_hitDirection == Direction.NorthWest)
                                _reflectionDirection = Direction.NorthEast;
                            _isReflecting = true;
                        }
                        else
                        {
                            _isReflecting = false;
                        }

                        break;
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
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        private void SetColor()
        {
            _color = new RayColor(Red, Green, Blue, 0.9f);
            _meshRenderer.material.color = _color.GetColor();
            FilterColors();
        }

        private void FilterColors()
        {
            var filteredColor = new RayColor(Red && _hitColor.R, Green && _hitColor.G, Blue && _hitColor.B, 0.9f);
            _filterRayEmitter.SetRayColor(filteredColor);

            var reboundColor = new RayColor(!Red && _hitColor.R, !Green && _hitColor.G, !Blue && _hitColor.B, 0.9f);
            _mirrorRayEmitter.SetRayColor(reboundColor);
        }

        public void HitEnter(Direction hitDirection, RayColor rayColor)
        {
            _hitColor = rayColor;
            FilterColors();

            // Mirror
            _hitDirection = hitDirection;
            _isHit = true;

            // Filter
            _filterRayEmitter.Enable(true);
            _hitDirection = hitDirection;
        }

        public void HitExit()
        {
            // Mirror
            _isHit = false;
            _mirrorRayEmitter.Enable(false);

            // Filter
            _filterRayEmitter.Enable(false);
        }
    }
}