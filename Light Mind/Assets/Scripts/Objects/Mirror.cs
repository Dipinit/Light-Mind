using System.Collections;
using System.Collections.Generic;
using Objects;
using UnityEngine;
using Utilities;


public class Mirror : MonoBehaviour, HitObject {

	private LineRenderer _lineRenderer;
	private Orientable _orientable;
	private Direction _reflectionDirection;
	private bool _isReflecting;
	private HitObject _hitObject;
	private RayColor _rayColor;
	

	// Use this for initialization
	void Start ()
	{
		_lineRenderer = GetComponent<LineRenderer>();
		_lineRenderer.enabled = false;

		_orientable = GetComponent<Orientable>();	
	}
	
	// Update is called once per frame
	void Update ()
	{
	}

	void RenderLaser()
	{
		// Set initial position
		_lineRenderer.SetPosition(0, transform.position);

		if (_isReflecting)
		{
			_lineRenderer.enabled = true;
			Gradient gradient = new Gradient();
			gradient.SetKeys(
				new GradientColorKey[] { new GradientColorKey(_rayColor.GetColor(), 0.0f), new GradientColorKey(_rayColor.GetColor(), 1.0f) },
				new GradientAlphaKey[] { new GradientAlphaKey(_rayColor.alpha, 0.0f), new GradientAlphaKey(_rayColor.alpha, 1.0f) }
			);
			_lineRenderer.colorGradient = gradient;

			Vector3 reflectionVector3 = Orientable.DirectionToVector3(_reflectionDirection);			
			
			// Check if laser hit an object
			RaycastHit hit;

			if (Physics.Raycast(transform.position, reflectionVector3, out hit))
			{
				if (hit.collider)
				{
					_lineRenderer.SetPosition(1, hit.point);
					HitObject obj = hit.transform.gameObject.GetComponent<HitObject>();
					if (obj != null)
					{
						if (_hitObject != null && obj != _hitObject)
						{
							_hitObject.HitExit();
						}

						_hitObject = obj;
						_hitObject.HitEnter(_reflectionDirection, _rayColor);
					}
				}
			}
			else
			{
				_lineRenderer.SetPosition(1, reflectionVector3 * 5000);
				if (_hitObject != null)
				{
					_hitObject.HitExit();
				}
			}
		}
		else
		{
			_lineRenderer.enabled = false;
		}
	}

	public void HitEnter(Direction hitDirection, RayColor rayColor)
	{
		switch (_orientable.Orientation)
		{
			case Direction.East:
				if (hitDirection == Direction.SouthWest || hitDirection == Direction.NorthWest)
				{
					if (hitDirection == Direction.SouthWest)
						_reflectionDirection = Direction.SouthEast;
					if (hitDirection == Direction.NorthWest)
						_reflectionDirection = Direction.NorthEast;
					_isReflecting = true;
				}
				else
				{
					_isReflecting = false;
				}
				break;
			case Direction.NorthEast:
				if (hitDirection == Direction.West || hitDirection == Direction.South)
				{
					if (hitDirection == Direction.West)
						_reflectionDirection = Direction.North;
					if (hitDirection == Direction.South)
						_reflectionDirection = Direction.East;
					_isReflecting = true;
				}
				else
				{
					_isReflecting = false;
				}
				break;
			case Direction.North:
				if (hitDirection == Direction.SouthEast || hitDirection == Direction.SouthWest)
				{
					if (hitDirection == Direction.SouthEast)
						_reflectionDirection = Direction.NorthEast;
					if (hitDirection == Direction.SouthWest)
						_reflectionDirection = Direction.NorthWest;
					_isReflecting = true;
				}
				else
				{
					_isReflecting = false;
				}
				break;
			case Direction.NorthWest:
				if (hitDirection == Direction.East || hitDirection == Direction.South)
				{
					if (hitDirection == Direction.East)
						_reflectionDirection = Direction.North;
					if (hitDirection == Direction.South)
						_reflectionDirection = Direction.West;
					_isReflecting = true;
				}
				else
				{
					_isReflecting = false;
				}
				break;
			case Direction.West:
				if (hitDirection == Direction.NorthEast || hitDirection == Direction.SouthEast)
				{
					if (hitDirection == Direction.NorthEast)
						_reflectionDirection = Direction.SouthEast;
					if (hitDirection == Direction.SouthEast)
						_reflectionDirection = Direction.NorthEast;
					_isReflecting = true;
				}
				else
				{
					_isReflecting = false;
				}
				break;
			case Direction.SouthWest:
				if (hitDirection == Direction.East || hitDirection == Direction.North)
				{
					if (hitDirection == Direction.East)
						_reflectionDirection = Direction.South;
					if (hitDirection == Direction.North)
						_reflectionDirection = Direction.West;
					_isReflecting = true;
				}
				else
				{
					_isReflecting = false;
				}
				break;
			case Direction.South:
				if (hitDirection == Direction.NorthWest || hitDirection == Direction.NorthEast)
				{
					if (hitDirection == Direction.NorthWest)
						_reflectionDirection = Direction.SouthWest;
					if (hitDirection == Direction.NorthEast)
						_reflectionDirection = Direction.NorthEast;
					_isReflecting = true;
				}
				else
				{
					_isReflecting = false;
				}
				break;
			case Direction.SouthEast:
				if (hitDirection == Direction.West || hitDirection == Direction.North)
				{
					if (hitDirection == Direction.West)
						_reflectionDirection = Direction.South;
					if (hitDirection == Direction.North)
						_reflectionDirection = Direction.East;
					_isReflecting = true;
				}
				else
				{
					_isReflecting = false;
				}
				break;		
		}

		_rayColor = rayColor;
		RenderLaser();
	}
	
	public void HitExit()
	{
		_lineRenderer.enabled = false;
	}

}

