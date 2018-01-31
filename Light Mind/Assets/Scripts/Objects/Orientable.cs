using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utilities;

public class Orientable : MonoBehaviour {
	
	public Direction Orientation;
	public float CatchTime = 0.25f;

	private Direction _orientation;
	private float _lastClickTime;

	
	// Use this for initialization
	void Start () {
		_orientation = Orientation;
		SetOrientation();
	}
	
	// Update is called once per frame
	void Update () {
		if (Orientation != _orientation)
			SetOrientation();
	}

	private void OnMouseUpAsButton()
	{
		// Detect double click
		if(Time.time - _lastClickTime < CatchTime)
		{
			Rotate();
		}
		_lastClickTime = Time.time;
	}

	void Rotate()
	{
		switch (Orientation)
		{
			case Direction.East:
				Orientation = Direction.NorthEast;
				break;
			case Direction.NorthEast:
				Orientation = Direction.North;
				break;
			case Direction.North:
				Orientation = Direction.NorthWest;
				break;
			case Direction.NorthWest:
				Orientation = Direction.West;
				break;
			case Direction.West:
				Orientation = Direction.SouthWest;
				break;
			case Direction.SouthWest:
				Orientation = Direction.South;
				break;
			case Direction.South:
				Orientation = Direction.SouthEast;
				break;
			case Direction.SouthEast:
				Orientation = Direction.East;
				break;
		}
	}

	void SetOrientation()
	{
		switch (Orientation)
		{
			case Direction.East:
				transform.eulerAngles = new Vector3(0, 0, 0);
				break;
			case Direction.NorthEast:
				transform.eulerAngles = new Vector3(0, 0, 45);
				break;
			case Direction.North:
				transform.eulerAngles = new Vector3(0, 0, 90);
				break;
			case Direction.NorthWest:
				transform.eulerAngles = new Vector3(0, 0, 135);
				break;
			case Direction.West:
				transform.eulerAngles = new Vector3(0, 0, 180);
				break;
			case Direction.SouthWest:
				transform.eulerAngles = new Vector3(0, 0, -135);
				break;
			case Direction.South:
				transform.eulerAngles = new Vector3(0, 0, -90);
				break;
			case Direction.SouthEast:
				transform.eulerAngles = new Vector3(0, 0, -45);
				break;
		}

		_orientation = Orientation;
	}

	public Vector3 GetVector3()
	{
		return DirectionToVector3(Orientation);
	}

	public static Vector3 DirectionToVector3(Direction direction)
	{
		switch (direction)
		{
			case Direction.East:
				return new Vector3(1, 0, 0);
			case Direction.NorthEast:
				return new Vector3(1, 1, 0);
			case Direction.North:
				return new Vector3(0, 1, 0);
			case Direction.NorthWest:
				return new Vector3(-1, 1, 0);
			case Direction.West:
				return new Vector3(-1, 0, 0);
			case Direction.SouthWest:
				return new Vector3(-1, -1, 0);
			case Direction.South:
				return new Vector3(0, -1, 0);
			case Direction.SouthEast:
				return new Vector3(1, -1, 0);
			default:
				return new Vector3(0, 0, 0);
		}
	}
}
