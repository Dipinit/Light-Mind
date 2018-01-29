using System;
using UnityEngine;

namespace Utilities
{
    public static class DirectionUtility
    {
        public static Vector3 getDirectionAsVector3(Direction direction)
        {
            var radians = (int) direction * Math.PI / 180;
            var x = (float) Math.Cos(radians);
            var y = (float) Math.Sin(radians);
            return new Vector3(x, y, 0);
        }
    }
}