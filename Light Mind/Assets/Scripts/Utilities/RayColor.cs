using System;
using UnityEngine;

namespace Assets.Scripts.Utilities
{
    [Serializable]
    public class RayColor
    {
        public bool R;
        public bool G;
        public bool B;
        public float Alpha;

        public RayColor(bool r, bool g, bool b, float alpha)
        {
            R = r;
            G = g;
            B = b;
            Alpha = alpha;
        }

        public Color GetColor()
        {
            return new Color(R ? 1F : 0F, G ? 1F : 0F, B ? 1F : 0F);
        }

        public override string ToString()
        {
            return string.Format("R:{0}, G:{1}, B:{2}",
                R ? "1" : "0",
                G ? "1" : "0",
                B ? "1" : "0");
        }

        public bool Equals(RayColor obj)
        {
            return obj.R == R && obj.G == G && obj.B == B && obj.Alpha == Alpha;
        }
    }
}