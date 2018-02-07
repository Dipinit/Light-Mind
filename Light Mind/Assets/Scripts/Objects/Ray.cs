using UnityEngine;
using Assets.Scripts.Utilities;
using Assets.Scripts.Objects;

public class Ray {
	
	private static int _idIcr = 0;
	
	// The line renderer used to draw rays
	public LineRenderer LineRenderer;

	// The current ray color
	public RayColor Color;

	// The current emitter state
	public bool Enabled;

	public Direction Direction;

	public RaySensitive RayEmitter;
	
	public RaySensitive RayReceiver;

	public GameObject LineRendererParent;

	public Ray Parent;

	public int Id;

	public Ray(RaySensitive rayEmitter, RayColor color, Direction direction, Ray parent)
	{
		RayEmitter = rayEmitter;
		LineRendererParent = new GameObject(string.Format("Ray {0} {1}",
			color,
			direction
		));
		LineRendererParent.transform.position = rayEmitter.transform.position;
		LineRendererParent.transform.rotation = rayEmitter.transform.rotation;
		LineRendererParent.transform.parent = rayEmitter.transform;
		LineRenderer = LineRendererParent.AddComponent<LineRenderer>();
		LineRenderer.startWidth = 0.3f;
		LineRenderer.endWidth = 0.3f;
		LineRenderer.material = new Material(Shader.Find("Sprites/Default"));
		LineRenderer.enabled = true;
		Color = color;
		Enabled = true;
		Direction = direction;
		RayReceiver = null;
		Parent = parent;
		Id = CreateId();
	}

	private static int CreateId()
	{
		_idIcr += 1;
		return _idIcr;
	}

	// Draw a line in a direction
	public void Emit()
	{
		//Debug.Log(string.Format("Ray receiver: {0}", RayReceiver));
		// Update the line renderer color
		SetGradientColor();

		// Set line initial position
		LineRenderer.SetPosition(0, LineRenderer.transform.position);

		// Set the line renderer position count to two positions (only one segment)
		LineRenderer.positionCount = 2;

		// Check if the ray hits an object in the input direction
		RaycastHit hit;
		if (Physics.Raycast(LineRenderer.transform.position, DirectionUtility.GetDirectionAsVector3(Direction),
			out hit))
		{
			// If the ray hits an object with a collider
			if (hit.collider)
			{
				// Set the end position to the object that was hit position
				LineRenderer.SetPosition(1,
					hit.point + 0.5F * DirectionUtility.GetDirectionAsVector3(Direction));

				// Check if the object that was hit is a HitObject
				var obj = hit.transform.gameObject;
				var rayReceiver = obj.GetComponent<RaySensitive>();
				if (rayReceiver == null || rayReceiver == RayReceiver) return;
				
				DeleteRayReceiver();
				
				// Store the new HitObject
				RayReceiver = rayReceiver;
				RayReceiver.HitEnter(this);

			}
			// If the ray hits an object with not collider and object and a current HitObject is set
			else
			{
				DeleteRayReceiver();
			}
		}
		// If the ray hits nothing
		else
		{
			// Draw a line in the input direction to the infinite
			LineRenderer.SetPosition(1, DirectionUtility.GetDirectionAsVector3(Direction) * 5000);
			DeleteRayReceiver();
		}
	}

	public void DeleteRayReceiver()
	{
		if (RayReceiver != null)
		{
			RayReceiver.HitExit(this);
			RayReceiver = null;
		}
	}

	// Set the line renderer color
	private void SetGradientColor()
	{
		// Set a gradient with the same color at the beginning and the end (we have to use a Gradient...)
		var gradient = new Gradient();
		gradient.SetKeys(
			new[]
			{
				new GradientColorKey(Color.GetColor(), 0.0f), new GradientColorKey(Color.GetColor(), 1.0f)
			},
			new[]
			{new GradientAlphaKey(Color.Alpha, 0.0f), new GradientAlphaKey(Color.Alpha, 1.0f)}
		);

		// Apply the gradient
		LineRenderer.colorGradient = gradient;
	}

	public static void Delete(Ray ray)
	{
		ray.DeleteRayReceiver();
		UnityEngine.Object.Destroy(ray.LineRendererParent);
	}
}
