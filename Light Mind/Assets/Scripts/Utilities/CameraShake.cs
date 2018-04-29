using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour {

    public static float shakeTimer;
    public static float shakeAmount;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (shakeTimer > 0)
        {
            Vector2 Shakepos = Random.insideUnitCircle * shakeAmount;
            transform.position = new Vector3(transform.position.x + Shakepos.x, transform.position.y + Shakepos.y, transform.position.z);

            shakeTimer -= Time.deltaTime;
        }
    }

    /// <summary>
    /// Shakes the camera.
    /// </summary>
    /// <param name="shakeAm"></param>
    /// <param name="shakeDur"></param>
    public void CamShake(float shakeAm, float shakeDur)
    {
        shakeAmount = shakeAm;
        shakeTimer = shakeDur;
    }
}
