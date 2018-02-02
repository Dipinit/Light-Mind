using System;
using Assets.Scripts.Utilities;
using UnityEngine;

namespace Assets.Scripts.Objects
{
    // Used to change the GameObjects direction
    public class Orientable : MonoBehaviour
    {
        // The direction setting
        public Direction Orientation;
        
        // The double click setting. Used to change the direction when double clicked 
        public float DoubleClickTime = 0.25f;

        // The current direction
        private Direction _orientation;
        
        // Last time the laser was clicked. Used to change the direction when double clicked 
        private float _lastClickTime;


        // Use this for initialization
        private void Start()
        {
            SetOrientation();
        }

        // Update is called once per frame
        private void Update()
        {
                SetOrientation();
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
        private void SetOrientation()
        {
            // If the orientation is new
            if (Orientation != _orientation)
            {
                // Update the object rotation based on the set orientation
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
                
                // Storing the value
                _orientation = Orientation;
            }
        }
    }
}