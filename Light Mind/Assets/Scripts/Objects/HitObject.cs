using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utilities;

public interface HitObject {

	// Use this for initialization
	void HitEnter(Direction hitDirection, RayColor rayColor);
	void HitExit();
}
