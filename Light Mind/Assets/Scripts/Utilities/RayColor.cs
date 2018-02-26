using System;
using UnityEngine;

namespace Assets.Scripts.Utilities
{
    [Serializable]
    public class RayColor
    {
        public static readonly float DEFAULT_ALPHA = 0.9f;
        public static readonly RayColor WHITE = new RayColor(true, true, true, DEFAULT_ALPHA, "White");
        public static readonly RayColor BLUE = new RayColor(false, false, true, DEFAULT_ALPHA, "Blue"); 
        public static readonly RayColor GREEN = new RayColor(false, true, false, DEFAULT_ALPHA, "Green"); 
        public static readonly RayColor RED = new RayColor(true, false, false, DEFAULT_ALPHA, "Red"); 
        public static readonly RayColor YELLOW = new RayColor(true, true, false, DEFAULT_ALPHA,"Yellow"); 
        public static readonly RayColor CYAN = new RayColor(false, true, true, DEFAULT_ALPHA,"Cyan"); 
        public static readonly RayColor MAGENTA = new RayColor(true, false, true, DEFAULT_ALPHA,"Magenta"); 
        public static readonly RayColor NONE = new RayColor(false, false, false, DEFAULT_ALPHA, "None");
        public static readonly RayColor[] COLORS = { WHITE, BLUE, GREEN, RED, YELLOW, CYAN, MAGENTA, NONE };
        public bool R;
        public bool G;
        public bool B;
        public float Alpha;
        public string Name;

        public RayColor(bool r, bool g, bool b, float alpha, string name = "color")
        {
            Name = name;
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