using Assets.Scripts.Utilities;
using UnityEngine;

namespace Items
{
    public class Obstacle : ItemBase
    {
        public override void Awake()
        {
            base.Awake();
            IsOrientable = false;
            IsColorable = false;
        }
    }
}