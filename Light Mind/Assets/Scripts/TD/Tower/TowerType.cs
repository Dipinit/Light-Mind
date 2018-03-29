﻿using System.Collections;
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

		public TowerTypes (float attackSpeed, float range, float AOE) {
			this.attackSpeed = attackSpeed;
			this.range = range;
			this.AOE = AOE;
		}

		public static readonly TowerTypes NORMAL = new TowerTypes(1.0f, 500.0f, 10.0f);
		public static readonly TowerTypes CANNON = new TowerTypes(0.5f, 500.0f, 75.0f);
	}
		

}
