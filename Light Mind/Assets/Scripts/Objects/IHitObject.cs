using Assets.Scripts.Utilities;

namespace Assets.Scripts.Objects
{
    public interface IHitObject
    {
        // Use this for initialization
        void HitEnter(Direction hitDirection, RayColor rayColor);
        void HitExit();
    }
}