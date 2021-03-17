using UnityEngine;

namespace Characters.Movements
{
	public abstract class Movement : ScriptableObject
	{
		public abstract Vector3 GetNextPosition(Transform transform, Vector3[] possibleDestinations);
	}
}