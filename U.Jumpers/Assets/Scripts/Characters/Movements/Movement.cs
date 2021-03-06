using UnityEngine;

namespace Characters.Movements
{
	public abstract class Movement : ScriptableObject
	{
		public abstract Vector3 GetNextPosition(Transform myTransform, Vector3[] possibleDestinations);
	}
}