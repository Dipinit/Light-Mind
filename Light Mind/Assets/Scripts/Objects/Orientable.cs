using System;
using Assets.Scripts.Utilities;
using UnityEngine;

namespace Assets.Scripts.Objects
{
    public class Orientable : MonoBehaviour
    {
        public Direction Orientation;
        public float DoubleClickTime = 0.25f;

        private Direction _orientation;
        private float _lastClickTime;


        // Use this for initialization
        private void Start()
        {
            _orientation = Orientation;
            SetOrientation();
        }

        // Update is called once per frame
        private void Update()
        {
            if (Orientation != _orientation)
                SetOrientation();
        }

        private void OnMouseUpAsButton()
        {
            // Detect double click
            if (Time.time - _lastClickTime < DoubleClickTime)
            {
                Rotate();
            }

            _lastClickTime = Time.time;
        }

        private void Rotate()
        {
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

        private void SetOrientation()
        {
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

            _orientation = Orientation;
        }
    }
}