using UnityEngine;
using Utilities;


public class Mirror : MonoBehaviour, HitObject {

	private Orientable _orientable;
	private Direction _reflectionDirection;
	private Direction _hitDirection;
	private bool _isReflecting;
	private bool _isHit;
	private RayEmitter _rayEmitter;

	// Use this for initialization
	void Start ()
	{
		_rayEmitter = new RayEmitter(GetComponent<LineRenderer>());
		_rayEmitter.Enable(false);

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
		_rayEmitter.Enable(_isReflecting);
		_rayEmitter.Emit(_reflectionDirection);
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
		_rayEmitter.SetRayColor(rayColor);
	}
	
	public void HitExit()
	{
		_isHit = false;
		_rayEmitter.Enable(false);
	}

}

