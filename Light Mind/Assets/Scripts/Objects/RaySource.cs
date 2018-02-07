using System;
using Assets.Scripts.Utilities;

namespace Assets.Scripts.Objects
{
    [Serializable]
    public class RaySource
    {
        public Direction Direction;
        public bool Enabled;
        public RayColor Color;

        public RaySource(Direction direction, bool enabled, RayColor color)
        {
            Direction = direction;
            Enabled = enabled;
            this.Color = color;
        }

        public bool Equals(RaySource obj)
        {
            return obj.Direction == Direction && obj.Enabled == Enabled && obj.Color.Equals(Color);
        }

        public override string ToString()
        {
            return string.Format("RaySource: {0} / {1} / {2}",
                Direction,
                Color,
                Enabled ? "Enabled" : "Disabled"
            );
        }
    }
}