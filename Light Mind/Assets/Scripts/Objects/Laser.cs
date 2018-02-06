using Assets.Scripts.Utilities;
using UnityEngine;

namespace Assets.Scripts.Objects
{
    // Ray source
    public class Laser : RaySensitive
    {
        
        // Public long click setting. Used to change the color of the source when holding click
        public float LongClickTime = 3f;
             
        // Last time the laser was clicked. Used to change the color of the source when holding click
        private float _lastClickTime;

        public override void Start()
        {
            base.Start();
            EmitNewRay(Direction.North, new RayColor(true, true, true, 0.9f), null);
        }
        
        // Update is called once per frame
        public override void Update()
        {
            base.Update();
        }
    }
}