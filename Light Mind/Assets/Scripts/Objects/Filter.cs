using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utilities;

public class Filter : MonoBehaviour, HitObject {
	
	private RayEmitter _rayEmitter;
	private Direction _hitDirection;
	private MeshRenderer _meshRenderer;
	private RayColor _color;
	private RayColor _hitColor;
	
	public bool red;
	public bool green;
	public bool blue;

	// Use this for initialization
	void Start () {
		_rayEmitter = new RayEmitter(GetComponent<LineRenderer>());
		_rayEmitter.Enable(false);
		_hitDirection = Direction.East;
		_meshRenderer = GetComponent<MeshRenderer>();
		_hitColor = new RayColor(true, true, true, 0.9f);
		SetColor();
	}
	
	// Update is called once per frame
	void Update () {
		if (_color.r != red || _color.g != green || _color.b != blue)
		{
			SetColor();
		}
		_rayEmitter.Emit(_hitDirection);
	}

	void SetColor()
	{
		_color = new RayColor(red, green, blue, 0.9f);
		_meshRenderer.material.color = _color.GetColor();
		FilterColor();
	}

	void FilterColor()
	{
		RayColor _filteredColor = new RayColor(red && _hitColor.r, green && _hitColor.g, blue && _hitColor.b, 0.9f);
		_rayEmitter.SetRayColor(_filteredColor);
	}

	public void HitEnter(Direction hitDirection, RayColor rayColor)
	{
		_rayEmitter.Enable(true);
		_hitColor = rayColor;
		FilterColor();
		_hitDirection = hitDirection;
	}

	public void HitExit()
	{
		_rayEmitter.Enable(false);
	}
}
