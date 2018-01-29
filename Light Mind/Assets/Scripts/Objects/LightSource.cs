using UnityEngine;
using Utilities;

namespace Objects
{
	public class LightSource : MonoBehaviour
	{
		public Direction EmitDirection;

		private Vector3 _direction;

		// Use this for initialization
		void Start ()
		{
			this._direction = DirectionUtility.getDirectionAsVector3(EmitDirection);
		}
	}
}
