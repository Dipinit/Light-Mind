using System;
using UnityEngine;

namespace Assets.Scripts.Utilities
{
    public static class DirectionUtility
    {
        public static Vector3 GetDirectionAsVector3(Direction direction)
        {
            var radians = (int) direction * Math.PI / 180;
            var x = (float) Math.Round(Math.Cos(radians));
            var y = (float) Math.Round(Math.Sin(radians));
            return new Vector3(x, 0, y);
        }

        public static Direction ToDirection(Vector3 dirVector3)
        {
            if (dirVector3 == new Vector3(1, 0, 0))
                return Direction.East;
            if (dirVector3 == new Vector3(1, 0, 1))
                return Direction.NorthEast;
            if (dirVector3 == new Vector3(0, 0, 1))
                return Direction.North;
            if (dirVector3 == new Vector3(-1, 0, 1))
                return Direction.NorthWest;
            if (dirVector3 == new Vector3(-1, 0, 0))
                return Direction.West;
            if (dirVector3 == new Vector3(-1, 0, -1))
                return Direction.SouthWest;
            if (dirVector3 == new Vector3(0, 0, -1))
                return Direction.South;
            if (dirVector3 == new Vector3(1, 0, -1))
                return Direction.SouthEast;
            throw new ArgumentException(string.Format("Invalid direction supplied: {0}", dirVector3), "dirVector3");
        }
    }
}