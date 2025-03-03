﻿using Assets.Scripts.Utilities;
using Behaviors;
using UnityEngine;

namespace Items
{
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

		public Vector3 VectorOffset = new Vector3(0.0f, 1.0f, 0.0f);

        /// <summary>
        /// Defined a ray
        /// </summary>
        /// <param name="rayEmitter">Source of the ray.</param>
        /// <param name="color">Color of the ray.</param>
        /// <param name="direction">Direction of the ray from the source.</param>
        /// <param name="parent">The ray received.</param>
		public Ray(RaySensitive rayEmitter, RayColor color, Direction direction, Ray parent)
		{
			RayEmitter = rayEmitter;
			LineRendererParent = new GameObject(string.Format("Ray {0} {1}",
				color,
				direction
			));
			LineRendererParent.transform.position = rayEmitter.transform.position + VectorOffset;
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

		public void Disable()
		{
			Enabled = false;
			LineRenderer.enabled = false;
		}

		public void Enable()
		{
			Enabled = true;
			LineRenderer.enabled = true;
        }

        /// <summary>
        /// Draw a line in a direction
        /// </summary>
		public void Emit()
		{
			if (Enabled)
			{

				if (LineRenderer.enabled != true)
					LineRenderer.enabled = true;

				//Debug.Log(string.Format("Ray receiver: {0}", RayReceiver));
				// Update the line renderer color
				SetGradientColor();

				// Set line initial position
				LineRenderer.SetPosition(0, LineRenderer.transform.position);

				// Set the line renderer position count to two positions (only one segment)
				LineRenderer.positionCount = 2;

				int layerMask = 1 << LayerMask.NameToLayer("ActiveItems");

				// Check if the ray hits an object in the input direction
				RaycastHit hit;
				if (Physics.Raycast(
					LineRenderer.transform.position,
					DirectionUtility.GetDirectionAsVector3(Direction),
					out hit,
					Mathf.Infinity,
					layerMask))
				{
					// If the ray hits an object with a collider
					if (hit.collider)
					{
						// Set the end position to the object that was hit position
						LineRenderer.SetPosition(1, hit.transform.position + VectorOffset);

						// Check if the object that was hit is a HitObject
						var rayReceiver = hit.transform.gameObject.GetComponent<RaySensitive>();
						
						// If the receiver hit by the ray is the current receiver
						if (rayReceiver != null && rayReceiver == RayReceiver) return;
						
						DeleteRayReceiver();

						if (rayReceiver != null)
						{
							// Store the new HitObject
							RayReceiver = rayReceiver;
							RayReceiver.HitEnter(this);
						}
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
			else
			{
				if (LineRenderer.enabled != false)
					LineRenderer.enabled = false;
			}
		}

        /// <summary>
        /// Delete a received ray.
        /// </summary>
		public void DeleteRayReceiver()
		{
			if (RayReceiver != null)
			{
				RayReceiver.HitExit(this);
				RayReceiver = null;
			}
		}

        /// <summary>
		/// Set the line renderer color
        /// </summary>
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
			if (ray.RayReceiver != null)
			{
				Debug.Log("RayReceiver is not null");
			}
			else
			{
				Debug.Log("RayReceiver is null");
			}
			ray.DeleteRayReceiver();
			UnityEngine.Object.Destroy(ray.LineRendererParent);
		}
	}
}
