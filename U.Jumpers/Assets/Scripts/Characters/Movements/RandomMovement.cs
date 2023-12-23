using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

namespace Characters.Movements
{
	[CreateAssetMenu(menuName = "Models/Movements/Random", fileName = "RandomMovement")]
	public class RandomMovement : Movement
	{
		public override Task<Vector3> GetNextPositionAsync(Pawn pawn)
		{
			Transform[] possibleDestinations = pawn.GetAvailablePillars(pawn.GetTransform.position);
			int selection = Random.Range(0, possibleDestinations.Length - 1);
			return Task.FromResult(possibleDestinations[selection].position);
		}
	}
}