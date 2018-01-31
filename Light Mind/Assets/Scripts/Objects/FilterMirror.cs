using UnityEngine;
using Utilities;


public class FilterMirror : MonoBehaviour, HitObject {

	//Global 
	private RayColor _hitColor;
	private RayColor _color;

	// Mirror
	private RayEmitter _mirrorRayEmitter;
	private Orientable _orientable;
	private Direction _reflectionDirection;
	private bool _isReflecting;
	private bool _isHit;
	
	// Filter
	private RayEmitter _filterRayEmitter;
	private Direction _hitDirection;
	private MeshRenderer _meshRenderer;
	
	public bool red;
	public bool green;
	public bool blue;

	// Use this for initialization
	void Start ()
	{
		// Mirror
		_mirrorRayEmitter = new RayEmitter(transform.Find("Mirror").GetComponent<LineRenderer>());
		_mirrorRayEmitter.Enable(false);
		_isReflecting = false;
		_isHit = false;
		_hitDirection = Direction.East;
		_reflectionDirection = Direction.East;
		_orientable = GetComponent<Orientable>();
		
		// Filter
		_filterRayEmitter = new RayEmitter(transform.Find("Filter").GetComponent<LineRenderer>());
		_filterRayEmitter.Enable(false);
		_meshRenderer = GetComponent<MeshRenderer>();
		_hitColor = new RayColor(true, true, true, 0.9f);
		SetColor();
	}
	
	void Update ()
	{
		// Mirror
		UpdateReflection();
		_mirrorRayEmitter.Enable(_isReflecting);
		_mirrorRayEmitter.Emit(_reflectionDirection);
		
		// Filter
		if (_color.r != red || _color.g != green || _color.b != blue)
		{
			SetColor();
		}
		_filterRayEmitter.Emit(_hitDirection);
	}

	// Mirror
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
							_reflectionDirection = Direction.SouthEast;
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
	
	void SetColor()
	{
		_color = new RayColor(red, green, blue, 0.9f);
		_meshRenderer.material.color = _color.GetColor();
		FilterColors();
	}

	void FilterColors()
	{
		RayColor _filteredColor = new RayColor(red && _hitColor.r, green && _hitColor.g, blue && _hitColor.b, 0.9f);
		_filterRayEmitter.SetRayColor(_filteredColor);
		
		RayColor _reboundColor = new RayColor(!red && _hitColor.r, !green && _hitColor.g, !blue && _hitColor.b, 0.9f);
		_mirrorRayEmitter.SetRayColor(_reboundColor);
	}

	public void HitEnter(Direction hitDirection, RayColor rayColor)
	{
		_hitColor = rayColor;
		FilterColors();
		
		// Mirror
		_hitDirection = hitDirection;
		_isHit = true;
		
		// Filter
		_filterRayEmitter.Enable(true);
		_hitDirection = hitDirection;
	}
	
	public void HitExit()
	{
		// Mirror
		_isHit = false;
		_mirrorRayEmitter.Enable(false);
		
		// Filter
		_filterRayEmitter.Enable(false);
	}

}

