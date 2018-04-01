using System;
using Assets.Scripts.Utilities;
using Behaviors;
using UnityEngine;

namespace Items
{
    // Used to change the GameObjects direction
    public abstract class ItemBase : RaySensitive
    {
        // Item orientation settings
        public bool IsOrientable = false;
        public Direction Orientation = Direction.East;
        
        // Item color settings
        public bool IsColorable = false;
        public RayColor Color = RayColor.WHITE;
        
        // The double click setting. Used to change the direction when double clicked 
        public float DoubleClickTime = 0.25f;

        // Current item orientation
        private Direction _orientation = Direction.None;
        
        // Current item color
        private RayColor _color = RayColor.NONE;

        // Last time the laser was clicked. Used to change the direction when double clicked 
        private float _lastClickTime;

        // Use this for initialization
        public override void Start()
        {
            base.Start();
            UpdateOrientation();
        }

        // Update is called once per frame
        public override void Update()
        {
            base.Update();
            UpdateOrientation();
            UpdateColor();
        }

        // Mouse up event
        private void OnMouseUpAsButton()
        {
            GameManager.Instance.SelectItem(this.gameObject);

            // Detect double click
            if (Time.time - _lastClickTime < DoubleClickTime)
            {
                // Rotate the object of 90° degrees
                Rotate();
            }


            _lastClickTime = Time.time;
        }

        // Rotate the object of 90° degrees
        private void Rotate()
        {
            if (!IsOrientable) return;
            
            // Changed the store orientation based on current direction
            switch (Orientation)
            {
                case Direction.East:
                    Orientation = Direction.NorthEast;
                    break;
                case Direction.NorthEast:
                    Orientation = Direction.North;
                    break;
                case Direction.North:
                    Orientation = Direction.NorthWest;
                    break;
                case Direction.NorthWest:
                    Orientation = Direction.West;
                    break;
                case Direction.West:
                    Orientation = Direction.SouthWest;
                    break;
                case Direction.SouthWest:
                    Orientation = Direction.South;
                    break;
                case Direction.South:
                    Orientation = Direction.SouthEast;
                    break;
                case Direction.SouthEast:
                    Orientation = Direction.East;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void UpdateColor()
        {
            // Update the RayColor if a color filter setting was changed
            if (_color.R != Color.R || _color.G != Color.G || _color.B != Color.B)
            {
                SetColor(Color);
            }
        }

        public virtual void SetColor(RayColor color)
        {
            Color = color;
            _color = color;
        }

        // Update the actual object rotation
        private void UpdateOrientation()
        {
            // If the orientation is not new, exit
            if (Orientation == _orientation) return;
            
            // Update the item rotation based on the previous orientation
            switch (Orientation)
            {
                case Direction.North:
                    transform.eulerAngles = new Vector3(0, 0, 0);
                    break;
                case Direction.NorthEast:
                    transform.eulerAngles = new Vector3(0, 45, 0);
                    break;
                case Direction.East:
                    transform.eulerAngles = new Vector3(0, 90, 0);
                    break;
                case Direction.SouthEast:
                    transform.eulerAngles = new Vector3(0, 135, 0);
                    break;
                case Direction.South:
                    transform.eulerAngles = new Vector3(0, 180, 0);
                    break;
                case Direction.SouthWest:
                    transform.eulerAngles = new Vector3(0, -135, 0);
                    break;
                case Direction.West:
                    transform.eulerAngles = new Vector3(0, -90, 0);
                    break;
                case Direction.NorthWest:
                    transform.eulerAngles = new Vector3(0, -45, 0);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
                
            // Store the new item orientation
            _orientation = Orientation;
            
            OnOrientationChange();
        }

        protected virtual void OnOrientationChange()
        {}
    }
}