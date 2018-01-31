using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utilities;

public class RayEmitter {

	private LineRenderer lineRenderer;
	private GameObject hitGameObject;
	private RayColor rayColor;
	private bool Enabled;

	public RayEmitter(LineRenderer lineRenderer)
	{
		this.lineRenderer = lineRenderer;
		this.rayColor = new RayColor(true, true, true, 1.0f);
		this.Enabled = true;
	}
	
	public RayEmitter(LineRenderer lineRenderer, RayColor rayColor)
	{
		this.lineRenderer = lineRenderer;
		this.rayColor = rayColor;
		this.Enabled = true;
	}

	public void Emit(Direction direction)
	{
	  	if (Enabled && (rayColor.r || rayColor.b || rayColor.g))
		{
			SetRayColor();
			
			if (!lineRenderer.enabled)
			{
				lineRenderer.enabled = true;
			}
			
			// Set initial position
			lineRenderer.SetPosition(0, lineRenderer.transform.position);
			lineRenderer.positionCount = 2;
	
			// Check if laser hit an object
			RaycastHit hit;
		   
			if (Physics.Raycast(lineRenderer.transform.position,  Orientable.DirectionToVector3(direction), out hit))
			{
				if (hit.collider)
				{
					lineRenderer.SetPosition(1, hit.point + 0.5F * Orientable.DirectionToVector3(direction));
					GameObject obj = hit.transform.gameObject;
					HitObject hitObject = obj.GetComponent<HitObject>();
					if (hitObject != null)
					{                        
						if (hitGameObject == null || (hitGameObject != null && obj != hitGameObject))
						{
							if (hitGameObject != null)
								hitGameObject.GetComponent<HitObject>().HitExit();
	
							hitGameObject = obj;
							Debug.Log("Ray hit " + hitGameObject.transform.parent.gameObject.ToString() + " " + hitGameObject.GetInstanceID() + " with direction " + direction.ToString() + " and color " + rayColor.GetColor());
							hitObject.HitEnter(direction, rayColor);
						}
					}
				}
				else if (hitGameObject != null)
				{
					Debug.Log("Ray stopped hitting " + hitGameObject.transform.parent.gameObject.ToString() + " " + hitGameObject.GetInstanceID() + " with direction " + direction.ToString());
					hitGameObject.GetComponent<HitObject>().HitExit();
					hitGameObject = null;
				}
			}
			else
			{
				lineRenderer.SetPosition(1, Orientable.DirectionToVector3(direction) * 5000);
				if (hitGameObject != null)
				{
					Debug.Log("Ray stopped hitting " + hitGameObject.transform.parent.gameObject.ToString() + " " + hitGameObject.GetInstanceID() + " with direction " + direction.ToString());
					hitGameObject.GetComponent<HitObject>().HitExit();
					hitGameObject = null;
				}
			}
		}
		else
		{
			if (lineRenderer.enabled)
			{
				lineRenderer.enabled = false;
			}
		}
	}

	public void Enable(bool boolean)
	{
		if (boolean)
		{
			Enabled = true;
		}
		else
		{
			Enabled = false;
			if (hitGameObject != null)
			{
				Debug.Log("Ray stopped hitting " + hitGameObject.transform.parent.gameObject.ToString() + " " + hitGameObject.GetInstanceID());
				hitGameObject.GetComponent<HitObject>().HitExit();
				hitGameObject = null;
			}
		}
	}
	
	void SetRayColor()
	{    
		Gradient gradient = new Gradient();
		gradient.SetKeys(
			new GradientColorKey[] { new GradientColorKey(rayColor.GetColor(), 0.0f), new GradientColorKey(rayColor.GetColor(), 1.0f) },
			new GradientAlphaKey[] { new GradientAlphaKey(rayColor.alpha, 0.0f), new GradientAlphaKey(rayColor.alpha, 1.0f) }
		);
		lineRenderer.colorGradient = gradient;
	}

	public void SetRayColor(RayColor rayColor)
	{
		if (rayColor != this.rayColor)
		{
			this.rayColor = rayColor;
			if (hitGameObject != null)
			{
				Debug.Log("Ray stopped hitting " + hitGameObject.transform.parent.gameObject.ToString() + " " + hitGameObject.GetInstanceID());
				hitGameObject.GetComponent<HitObject>().HitExit();
				hitGameObject = null;
			}
		}
	}
}
