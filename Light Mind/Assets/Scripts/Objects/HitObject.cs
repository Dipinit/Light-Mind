using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface HitObject {

	// Use this for initialization
	void hitEnter(Vector3 direction);
	void hitExit();
}
