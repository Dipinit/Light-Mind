using System;
using NUnit.Framework.Constraints;
using UnityEngine;
using UnityEngine.Experimental.UIElements;

namespace Assets.Scripts.Utilities
{
    [Serializable]
    public class RayColor
    {
        public static readonly float DEFAULT_ALPHA = 0.9f;
        public static readonly RayColor WHITE = new RayColor(true, true, true, DEFAULT_ALPHA); 
        public static readonly RayColor BLUE = new RayColor(false, false, true, DEFAULT_ALPHA); 
        public static readonly RayColor GREEN = new RayColor(false, true, false, DEFAULT_ALPHA); 
        public static readonly RayColor RED = new RayColor(true, false, false, DEFAULT_ALPHA); 
        public static readonly RayColor YELLOW = new RayColor(true, true, false, DEFAULT_ALPHA); 
        public static readonly RayColor CYAN = new RayColor(false, true, true, DEFAULT_ALPHA); 
        public static readonly RayColor MAGENTA = new RayColor(true, false, true, DEFAULT_ALPHA); 
        public static readonly RayColor NONE = new RayColor(false, false, false, DEFAULT_ALPHA);
        
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