using UnityEngine;
using Utilities;

public class Prism : MonoBehaviour, HitObject
{

	private LineRenderer _blueLineRenderer;
	private LineRenderer _redLineRenderer;
	private LineRenderer _greenLineRenderer;
	private GameObject _blueHitGameObject;
	private GameObject _redHitGameObject;
	private GameObject _greenHitGameObject;
	private Direction _hitDirection;

	private RayColor _rayColor;
	
	// Use this for initialization
	void Start ()
	{
		Gradient gradient = new Gradient();

		_blueLineRenderer = transform.Find("Blue").GetComponent<LineRenderer>();
		gradient.SetKeys(
			new GradientColorKey[] { new GradientColorKey(Color.blue, 0.0f), new GradientColorKey(Color.blue, 1.0f) },
			new GradientAlphaKey[] { new GradientAlphaKey(0.9f, 0.0f), new GradientAlphaKey(0.9f, 1.0f) }
		);
		_blueLineRenderer.colorGradient = gradient;
		
		_redLineRenderer = transform.Find("Red").GetComponent<LineRenderer>();
		gradient.SetKeys(
			new GradientColorKey[] { new GradientColorKey(Color.red, 0.0f), new GradientColorKey(Color.red, 1.0f) },
			new GradientAlphaKey[] { new GradientAlphaKey(0.9f, 0.0f), new GradientAlphaKey(0.9f, 1.0f) }
		);
		_redLineRenderer.colorGradient = gradient;
		
		_greenLineRenderer = transform.Find("Green").GetComponent<LineRenderer>();
		gradient.SetKeys(
			new GradientColorKey[] { new GradientColorKey(Color.green, 0.0f), new GradientColorKey(Color.green, 1.0f) },
			new GradientAlphaKey[] { new GradientAlphaKey(0.9f, 0.0f), new GradientAlphaKey(0.9f, 1.0f) }
		);
		_greenLineRenderer.colorGradient = gradient;
	}
	
	// Update is called once per frame
	void Update ()
	{
		RenderLasers();
	}

	void RenderLasers()
	{
		if (_blueLineRenderer.enabled)
			RenderBlueLaser();
		if (_greenLineRenderer.enabled)
			RenderGreenLaser();
		if (_redLineRenderer.enabled)
			RenderRedLaser();
	}

	void RenderBlueLaser()
	{
		Vector3 direction = Orientable.DirectionToVector3(_hitDirection);
		direction = Quaternion.AngleAxis(90, Vector3.forward) * direction;
		  
		// Set initial position
		_blueLineRenderer.SetPosition(0, transform.position);
		_blueLineRenderer.positionCount = 2;

		// Check if laser hit an object
		RaycastHit hit;
               
		if (Physics.Raycast(transform.position,  direction, out hit))
		{
			if (hit.collider)
			{
				_blueLineRenderer.SetPosition(1, hit.point + 0.5F * direction);
				
				GameObject obj = hit.transform.gameObject;
				HitObject hitObject = obj.GetComponent<HitObject>();
				if (hitObject != null)
				{                        
					if (_blueHitGameObject == null || (_blueHitGameObject != null && obj != _blueHitGameObject))
					{
						if (_blueHitGameObject != null)
							_blueHitGameObject.GetComponent<HitObject>().HitExit();

						_blueHitGameObject = obj;
						Debug.Log("Laser hit " + _blueHitGameObject.transform.parent.gameObject.ToString() + " " + _blueHitGameObject.GetInstanceID() + " with direction " + _blueHitGameObject.ToString() + " and color " + new RayColor(false, true, false, 0.9f));
						hitObject.HitEnter(Orientable.Vector3ToDirection(direction), new RayColor(false, false, true, 0.9f));
					}
				}
			}
		}
		else
		{
			_blueLineRenderer.SetPosition(1, direction * 5000);
			if (_blueHitGameObject != null)
			{
				_blueHitGameObject.GetComponent<HitObject>().HitExit();
			}

			_blueHitGameObject = null;
		}
	}
	
	void RenderGreenLaser()
	{
		Vector3 direction = Orientable.DirectionToVector3(_hitDirection);
		direction = Quaternion.AngleAxis(-90, Vector3.forward) * direction;
                
		// Set initial position
		_greenLineRenderer.SetPosition(0, transform.position);
		_greenLineRenderer.positionCount = 2;

		// Check if laser hit an object
		RaycastHit hit;
               
		if (Physics.Raycast(transform.position,  direction, out hit))
		{
			if (hit.collider)
			{
				_greenLineRenderer.SetPosition(1, hit.point + 0.5F * direction);
				
				GameObject obj = hit.transform.gameObject;
				HitObject hitObject = obj.GetComponent<HitObject>();
				if (hitObject != null)
				{                        
					if (_greenHitGameObject == null || (_greenHitGameObject != null && obj != _greenHitGameObject))
					{
						if (_greenHitGameObject != null)
							_greenHitGameObject.GetComponent<HitObject>().HitExit();

						_greenHitGameObject = obj;
						Debug.Log("Laser hit " + _greenHitGameObject.transform.parent.gameObject.ToString() + " " + _greenHitGameObject.GetInstanceID() + " with direction " + _greenHitGameObject.ToString() + " and color " + new RayColor(false, true, false, 0.9f));
						hitObject.HitEnter(Orientable.Vector3ToDirection(direction), new RayColor(false, true, false, 0.9f));
					}
				}
			}
		}
		else
		{
			_greenLineRenderer.SetPosition(1, direction * 5000);
			if (_greenHitGameObject != null)
			{
				_greenHitGameObject.GetComponent<HitObject>().HitExit();
			}

			_greenHitGameObject = null;
		}
	}
	
	void RenderRedLaser()
	{
		Vector3 direction = Orientable.DirectionToVector3(_hitDirection);
		direction = Quaternion.AngleAxis(0, Vector3.forward) * direction;

		// Set initial position
		_redLineRenderer.SetPosition(0, transform.position);
		_redLineRenderer.positionCount = 2;

		// Check if laser hit an object
		RaycastHit hit;
               
		if (Physics.Raycast(transform.position,  direction, out hit))
		{
			if (hit.collider)
			{
				_redLineRenderer.SetPosition(1, hit.point + 0.5F * direction);
				
				GameObject obj = hit.transform.gameObject;
				HitObject hitObject = obj.GetComponent<HitObject>();
				if (hitObject != null)
				{                        
					if (_redHitGameObject == null || (_redHitGameObject != null && obj != _redHitGameObject))
					{
						if (_redHitGameObject != null)
							_redHitGameObject.GetComponent<HitObject>().HitExit();

						_redHitGameObject = obj;
						Debug.Log("Laser hit " + _redHitGameObject.transform.parent.gameObject.ToString() + " " + _redHitGameObject.GetInstanceID() + " with direction " + _redHitGameObject.ToString() + " and color " + new RayColor(false, true, false, 0.9f));
						hitObject.HitEnter(Orientable.Vector3ToDirection(direction), new RayColor(true, false, false, 0.9f));
					}
				}
			}
		}
		else
		{
			_redLineRenderer.SetPosition(1, direction * 5000);
			if (_redHitGameObject != null)
			{
				_redHitGameObject.GetComponent<HitObject>().HitExit();
			}

			_redHitGameObject = null;
		}
	}

	public void HitEnter(Direction hitDirection, RayColor rayColor)
	{
		_hitDirection = hitDirection;
		_blueLineRenderer.enabled = rayColor.b;
		_greenLineRenderer.enabled = rayColor.g;
		_redLineRenderer.enabled = rayColor.r;
	}

	public void HitExit()
	{
		_blueLineRenderer.enabled = false;
		_greenLineRenderer.enabled = false;
		_redLineRenderer.enabled = false;
	}
}
