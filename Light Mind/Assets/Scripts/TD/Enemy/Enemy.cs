using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Utilities;

public class Enemy : MonoBehaviour {

    private List<Vector3> _paths = new List<Vector3>();
    private int _currPath = 0;
    private float _speed = 1.2f;

    public RayColor EnemyColor;

	// Use this for initialization
	void Start () {
        Vector3 destination = GameObject.FindObjectOfType<DiamontBehaviour> ().transform.position;
        _paths.Add (destination);
	}

    public void Init(List<Vector3> paths, RayColor color) {
        _currPath = 0;
        _paths = paths;
        EnemyColor = color;
        SpriteRenderer renderer = GetComponent<SpriteRenderer>();
        renderer.color = color.GetColor ();
    }
	
	// Update is called once per frame
	void Update () {
        /* @THOMAS
         * Les enemies arrivent pas au milieu mais vienne juste touche le checkpoint donc des fois ils vont en diagonale
         * Soit faut faire des map bien rectangulaires et tout, soit faut fixsi ca t'intéresses.
         **/
        if (_paths != null && _paths.Count > 0 && _currPath < _paths.Count) {
            Vector3 nextPath = _paths [_currPath];
            nextPath.z = -1;
           if (Vector3.Distance (transform.position,nextPath) > 0.01) {
                transform.position += (nextPath-transform.position).normalized*Time.deltaTime*_speed;
            } else {
                _currPath++;
            }
        } else {
            // CHECK COLLIDER
        }
	}
}
