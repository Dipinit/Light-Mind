using System.Collections;
using System.Collections.Generic;
using Objects;
using UnityEngine;
using Utilities;


public class Mirror : MonoBehaviour, HitObject {

	public Direction Direction;

	private Vector3 _reflectionDirection;
	private MeshCollider _meshCollider;
	private Laser _laser;

	// Use this for initialization
	void Start ()
	{
		_meshCollider = GetComponentInChildren<MeshCollider>();
		_laser = GetComponentInChildren<Laser>();
		_reflectionDirection = DirectionUtility.getDirectionAsVector3(Direction);
	}
	// Update is called once per frame
	void Update () {

	}

	public void hitEnter(Vector3 direction)
	{
		Debug.Log("HIT ENTER SA MERE");
		_laser.On = true;
	}
	
	public void hitExit()
	{
		Debug.Log("HIT EXIT SA MERE");
		_laser.On = false;
	}
}

