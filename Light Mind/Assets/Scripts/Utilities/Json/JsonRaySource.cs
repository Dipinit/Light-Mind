namespace Assets.Scripts.Utilities.Json
{
    [System.Serializable]
    public class JsonRaySource
    {
        public Direction Direction;
        public bool Enabled;
        public bool Red;
        public bool Green;
        public bool Blue;

        public override string ToString()
        {
            return string.Format(
                "{0} / {1} / R{2}:G{3}:B{4}",
                Direction,
                Enabled,
                Red ? 1 : 0,
                Green ? 1 : 0,
                Blue ? 1 : 0
            );
        }
    }
}