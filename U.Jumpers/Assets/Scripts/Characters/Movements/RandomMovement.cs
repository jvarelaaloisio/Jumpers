using UnityEngine;

namespace Characters.Movements
{
	[CreateAssetMenu(menuName = "Models/Movements/Random", fileName = "RandomMovement")]
	class RandomMovement : Movement
	{
		public override Vector3 GetNextPosition(Transform transform, Vector3[] possibleDestinations)
		{
			int selection = Random.Range(0, possibleDestinations.Length - 1);
			return possibleDestinations[selection];
		}
	}
}