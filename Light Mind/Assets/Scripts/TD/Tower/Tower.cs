using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Items;
using Assets.Scripts.Utilities;

namespace Items
{
	public class Tower : ItemBase
	{
		public RayColor color = RayColor.WHITE;
		public TowerTypes towerType = TowerTypes.NORMAL;
		// Use this for initialization
		public override void Awake()
		{
			IsOrientable = true;
			IsColorable = false;
		}

        /// <summary>
        /// Effect - Recolor tower to boost dmg
        /// </summary>
        /// <param name="ray"></param>
		public override void HandleReceivedRay(Ray ray)
		{
			this.color = ray.Color;
            //TODO Recolor
		}

		// Launched when a ray hits the mirror
		public override void HitEnter(Ray ray)
		{
			HandleReceivedRay(ray);
		}
	}
}
