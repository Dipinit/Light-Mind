using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Assets.Scripts.Utilities
{
	[Serializable]
	public class TowerTypes {
		public float attackSpeed;
		public float range;
		public float AOE;

        /// <summary>
        /// Set properties of a tower.
        /// </summary>
        /// <param name="attackSpeed">Cool time before next attack.</param>
        /// <param name="range">Range of the attack.</param>
        /// <param name="AOE"></param>
		public TowerTypes (float attackSpeed, float range, float AOE) {
			this.attackSpeed = attackSpeed;
			this.range = range;
			this.AOE = AOE;
		}

		public static readonly TowerTypes NORMAL = new TowerTypes(1.0f, 500.0f, 10.0f);
		public static readonly TowerTypes CANNON = new TowerTypes(0.5f, 500.0f, 75.0f);
	}
		

}
