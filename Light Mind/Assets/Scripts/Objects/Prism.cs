using UnityEngine;
using Utilities;

public class Prism : MonoBehaviour, HitObject
{
	private RayEmitter _blueRayEmitter;
	private RayEmitter _redRayEmitter;
	private RayEmitter _greenRayEmitter;

	private Direction _hitDirection;
	private RayColor _rayColor;
	
	// Use this for initialization
	void Start ()
	{
		_blueRayEmitter = new RayEmitter(transform.Find("Blue").GetComponent<LineRenderer>(), new RayColor(false, false, true, 0.9f));
		_blueRayEmitter.Enable(false);
		
		_greenRayEmitter = new RayEmitter(transform.Find("Green").GetComponent<LineRenderer>(), new RayColor(false, true, false, 0.9f));
		_greenRayEmitter.Enable(false);
		
		_redRayEmitter = new RayEmitter(transform.Find("Red").GetComponent<LineRenderer>(), new RayColor(true, false, false, 0.9f));
		_redRayEmitter.Enable(false);
	}
	
	// Update is called once per frame
	void Update ()
	{
		RenderLasers();
	}

	void RenderLasers()
	{
		RenderBlueLaser();
		RenderGreenLaser();
		RenderRedLaser();
	}

	void RenderBlueLaser()
	{
		Vector3 direction = Orientable.DirectionToVector3(_hitDirection);
		direction = Quaternion.AngleAxis(90, Vector3.forward) * direction;
		
		_blueRayEmitter.Emit(Orientable.Vector3ToDirection(direction));
	}
	
	void RenderRedLaser()
	{
		Vector3 direction = Orientable.DirectionToVector3(_hitDirection);
		direction = Quaternion.AngleAxis(0, Vector3.forward) * direction;
		
		_redRayEmitter.Emit(Orientable.Vector3ToDirection(direction));
	}
	
	void RenderGreenLaser()
	{
		Vector3 direction = Orientable.DirectionToVector3(_hitDirection);
		direction = Quaternion.AngleAxis(-90, Vector3.forward) * direction;
		
		_greenRayEmitter.Emit(Orientable.Vector3ToDirection(direction));
	}
	
	public void HitEnter(Direction hitDirection, RayColor rayColor)
	{
		_hitDirection = hitDirection;
		_blueRayEmitter.Enable(rayColor.b);
		_greenRayEmitter.Enable(rayColor.g);
		_redRayEmitter.Enable(rayColor.r);
	}

	public void HitExit()
	{
		_blueRayEmitter.Enable(false);
		_greenRayEmitter.Enable(false);
		_redRayEmitter.Enable(false);
	}
}
