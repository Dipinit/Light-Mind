using System;
using Assets.Scripts.Utilities;
using Behaviors;
using UnityEngine;

namespace Items
{
    // Used to change the GameObjects direction
    public abstract class ItemBase : RaySensitive
    {
        // Item orientation setting
        public Direction Orientation = Direction.East;
        
        // The double click setting. Used to change the direction when double clicked 
        public float DoubleClickTime = 0.25f;

        // Current item orientation
        private Direction _orientation = Direction.None;
        
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
        }

        // Mouse up event
        private void OnMouseUpAsButton()
        {
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

        // Update the actual object rotation
        private void UpdateOrientation()
        {
            // If the orientation is not new, exit
            if (Orientation == _orientation) return;
            
            // Update the item rotation based on the previous orientation
            switch (Orientation)
            {
                case Direction.East:
                    transform.eulerAngles = new Vector3(0, 0, 0);
                    break;
                case Direction.NorthEast:
                    transform.eulerAngles = new Vector3(0, 0, 45);
                    break;
                case Direction.North:
                    transform.eulerAngles = new Vector3(0, 0, 90);
                    break;
                case Direction.NorthWest:
                    transform.eulerAngles = new Vector3(0, 0, 135);
                    break;
                case Direction.West:
                    transform.eulerAngles = new Vector3(0, 0, 180);
                    break;
                case Direction.SouthWest:
                    transform.eulerAngles = new Vector3(0, 0, -135);
                    break;
                case Direction.South:
                    transform.eulerAngles = new Vector3(0, 0, -90);
                    break;
                case Direction.SouthEast:
                    transform.eulerAngles = new Vector3(0, 0, -45);
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