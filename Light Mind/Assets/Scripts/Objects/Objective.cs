using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utilities;

public class Objective : MonoBehaviour, HitObject {
	public bool red;
	public bool green;
	public bool blue;
	public bool completed;

	private RayColor _color;
	private RayColor _hitColor;
	private MeshRenderer _meshRenderer;
	
	// Use this for initialization
	void Start () {
		_meshRenderer = GetComponent<MeshRenderer>();
		SetColor();
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (_color.r != red || _color.g != green || _color.b != blue)
		{
			SetColor();
		}
		CheckCompletion();
	}

	public void HitEnter(Direction hitDirection, RayColor rayColor)
	{
		_hitColor = rayColor;
	}

	public void HitExit()
	{
		_hitColor = null;
	}

	void SetColor()
	{
		_color = new RayColor(red, green, blue, 0.9f);
		_meshRenderer.material.color = _color.GetColor();
	}
	
	void CheckCompletion()
	{
		if (_hitColor != null && _hitColor.r == _color.r && _hitColor.g == _color.g && _hitColor.b == _color.b)
		{
			if (completed != true)
			{
				Debug.Log("Objective " + transform.gameObject.GetInstanceID() + " is completed");
				completed = true;
			}
		}
		else
		{
			if (completed != false)
			{
				Debug.Log("Objective " + transform.gameObject.GetInstanceID() + " is not completed anymore");
				completed = false;
			}
		}
	}
}
