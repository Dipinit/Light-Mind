using System;
using UnityEngine;
using System.Collections;
using Assets.Scripts.Utilities;

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
        public static readonly RayColor[] COLORS = { WHITE, BLUE, GREEN, RED, YELLOW, CYAN, MAGENTA };
        
        public bool R;
        public bool G;
        public bool B;
        public float Alpha;

        /// <summary>
        /// Set the color of a ray.
        /// </summary>
        /// <param name="r">Is the ray composed of red ?</param>
        /// <param name="g">Is the ray composed of green ?</param>
        /// <param name="b">Is the ray composed of blue ?</param>
        /// <param name="alpha">Amount of alpha.</param>
        public RayColor(bool r, bool g, bool b, float alpha)
        {
            R = r;
            G = g;
            B = b;
            Alpha = alpha;
        }

        /// <summary>
        /// Get the color of a ray.
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// Get the color of a ray.
        /// </summary>
        /// <returns></returns>
        public string GetName()
        {
            if (R && G && B) return "White";
            if (!R && !G && B) return "Blue";
            if (!R && G && !B) return "Green";
            if (R && !G && !B) return "Red";
            if (R && G && !B) return "Yellow";
            if (!R && G && B) return "Cyan";
            if (R && !G && B) return "Magenta";
            return "None";
        }

        /// <summary>
        /// Add a color to a ray.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static RayColor Add(RayColor x, RayColor y)
        {
            return new RayColor(x.R || y.R, x.G || y.G, x.B || y.B, (x.Alpha + y.Alpha) / 2);
        }
        
        /// <summary>
        /// Eliminate a color of a ray.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static RayColor Substract(RayColor x, RayColor y)
        {
            return new RayColor(x.R && !y.R, x.G && !y.G, x.B && !y.B, (x.Alpha + y.Alpha) / 2);
        }

        /// <summary>
        /// Parse a ray.
        /// </summary>
        /// <param name="color"></param>
        /// <returns></returns>
        public static RayColor Parse(string color) {
            if (color != null && color.Length > 0) {
                switch (color.ToLower ())
                {
                    case "white":
                        return RayColor.WHITE;
                    case "blue":
                        return RayColor.BLUE;
                    case "green":
                        return RayColor.GREEN;
                    case "red":
                        return RayColor.RED;
                    case "yellow":
                        return RayColor.YELLOW;
                    case "cyan":
                        return RayColor.CYAN;
                    case "magenta":
                        return RayColor.MAGENTA;
                    default:
                        return RayColor.NONE;
                } 
            }
            Debug.Log ("RayColor.Parse() - Looks like color is null or empty, couldn't convert it!");
            return RayColor.NONE;
        }
    }
}