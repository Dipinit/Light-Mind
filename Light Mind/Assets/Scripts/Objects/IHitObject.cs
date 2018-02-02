using Assets.Scripts.Utilities;

namespace Assets.Scripts.Objects
{
    public interface IHitObject
    {
        // Launched when a ray hits the object
        void HitEnter(Direction hitDirection, RayColor rayColor);
        
        // Launched when a ray stops hitting the object
        void HitExit();
    }
}