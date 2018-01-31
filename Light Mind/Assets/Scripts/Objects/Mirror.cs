using System.Collections;
using System.Collections.Generic;
using Objects;
using UnityEngine;
using Utilities;


public class Mirror : MonoBehaviour, HitObject {

	private LineRenderer _lineRenderer;
	private Orientable _orientable;
	private Direction _reflectionDirection;
	private Direction _hitDirection;
	private bool _isReflecting;
	private bool _isHit;
	private GameObject _hitGameObject;
	private RayColor _rayColor;
	

	// Use this for initialization
	void Start ()
	{
		_lineRenderer = GetComponent<LineRenderer>();
		_lineRenderer.enabled = false;

		_isReflecting = false;
		_isHit = false;
		_hitDirection = Direction.East;
		_reflectionDirection = Direction.East;

		_orientable = GetComponent<Orientable>();	
	}
	
	// Update is called once per frame
	void Update ()
	{
		UpdateReflection();
		RenderLaser();
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
					
					GameObject obj = hit.transform.gameObject;
					HitObject hitObject = obj.GetComponent<HitObject>();
					if (hitObject != null)
					{                        
						if (_hitGameObject == null || (_hitGameObject != null && obj != _hitGameObject))
						{
							if (_hitGameObject != null)
								_hitGameObject.GetComponent<HitObject>().HitExit();

							_hitGameObject = obj;
							Debug.Log("Laser hit " + _hitGameObject.transform.parent.gameObject.ToString() + " " + _hitGameObject.GetInstanceID() + " with direction " + _reflectionDirection.ToString() + " and color " + _rayColor.GetColor());
							hitObject.HitEnter(_reflectionDirection, _rayColor);
						}
					}
				}
			}
			else
			{
				_lineRenderer.SetPosition(1, reflectionVector3 * 5000);
				if (_hitGameObject != null)
				{
					_hitGameObject.GetComponent<HitObject>().HitExit();
				}
				_hitGameObject = null;

			}
		}
		else
		{
			_lineRenderer.enabled = false;
		}
	}

	void UpdateReflection()
	{
		if (!_isHit)
		{
			_isReflecting = false;
		}
		else
		{
			switch (_orientable.Orientation)
			{
				case Direction.East:
					if (_hitDirection == Direction.SouthWest || _hitDirection == Direction.NorthWest)
					{
						if (_hitDirection == Direction.SouthWest)
							_reflectionDirection = Direction.SouthEast;
						if (_hitDirection == Direction.NorthWest)
							_reflectionDirection = Direction.NorthEast;
						_isReflecting = true;
					}
					else
					{
						_isReflecting = false;
					}

					break;
				case Direction.NorthEast:
					if (_hitDirection == Direction.West || _hitDirection == Direction.South)
					{
						if (_hitDirection == Direction.West)
							_reflectionDirection = Direction.North;
						if (_hitDirection == Direction.South)
							_reflectionDirection = Direction.East;
						_isReflecting = true;
					}
					else
					{
						_isReflecting = false;
					}

					break;
				case Direction.North:
					if (_hitDirection == Direction.SouthEast || _hitDirection == Direction.SouthWest)
					{
						if (_hitDirection == Direction.SouthEast)
							_reflectionDirection = Direction.NorthEast;
						if (_hitDirection == Direction.SouthWest)
							_reflectionDirection = Direction.NorthWest;
						_isReflecting = true;
					}
					else
					{
						_isReflecting = false;
					}

					break;
				case Direction.NorthWest:
					if (_hitDirection == Direction.East || _hitDirection == Direction.South)
					{
						if (_hitDirection == Direction.East)
							_reflectionDirection = Direction.North;
						if (_hitDirection == Direction.South)
							_reflectionDirection = Direction.West;
						_isReflecting = true;
					}
					else
					{
						_isReflecting = false;
					}

					break;
				case Direction.West:
					if (_hitDirection == Direction.NorthEast || _hitDirection == Direction.SouthEast)
					{
						if (_hitDirection == Direction.NorthEast)
							_reflectionDirection = Direction.SouthEast;
						if (_hitDirection == Direction.SouthEast)
							_reflectionDirection = Direction.NorthEast;
						_isReflecting = true;
					}
					else
					{
						_isReflecting = false;
					}

					break;
				case Direction.SouthWest:
					if (_hitDirection == Direction.East || _hitDirection == Direction.North)
					{
						if (_hitDirection == Direction.East)
							_reflectionDirection = Direction.South;
						if (_hitDirection == Direction.North)
							_reflectionDirection = Direction.West;
						_isReflecting = true;
					}
					else
					{
						_isReflecting = false;
					}

					break;
				case Direction.South:
					if (_hitDirection == Direction.NorthWest || _hitDirection == Direction.NorthEast)
					{
						if (_hitDirection == Direction.NorthWest)
							_reflectionDirection = Direction.SouthWest;
						if (_hitDirection == Direction.NorthEast)
							_reflectionDirection = Direction.NorthEast;
						_isReflecting = true;
					}
					else
					{
						_isReflecting = false;
					}

					break;
				case Direction.SouthEast:
					if (_hitDirection == Direction.West || _hitDirection == Direction.North)
					{
						if (_hitDirection == Direction.West)
							_reflectionDirection = Direction.South;
						if (_hitDirection == Direction.North)
							_reflectionDirection = Direction.East;
						_isReflecting = true;
					}
					else
					{
						_isReflecting = false;
					}

					break;
			}
		}
	}

	public void HitEnter(Direction hitDirection, RayColor rayColor)
	{
		_hitDirection = hitDirection;
		_isHit = true;
		_rayColor = rayColor;
	}
	
	public void HitExit()
	{
		_isHit = false;
	}

}

