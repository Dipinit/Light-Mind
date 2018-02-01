using UnityEngine;

namespace Assets.Scripts.Utilities
{
    public class RayColor
    {
        public readonly bool R;
        public readonly bool G;
        public readonly bool B;
        public readonly float Alpha;

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
    }
}