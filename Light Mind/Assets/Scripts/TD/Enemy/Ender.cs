using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ender : MonoBehaviour {

    public TDManager TDManager;
	
    void Start() {
        TDManager = GameObject.FindObjectOfType<TDManager> ();
    }

	void Update () {
		
	}

    // if gameobject enemy touches Ender gameobject, inform TDManager
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag.Equals ("enemy")) {
            TDManager.DecreaseLives ();
            Destroy (other.gameObject);
        }
    }
    
}
