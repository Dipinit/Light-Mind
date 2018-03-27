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
    void OnTriggerEnter(Collider co) {
        if (co.gameObject.name == "EnemyPrefab") {
            TDManager.DecreaseLives ();
        }
    }
    
}
