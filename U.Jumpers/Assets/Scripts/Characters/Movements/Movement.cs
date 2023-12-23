using System.Threading.Tasks;
using UnityEngine;

namespace Characters.Movements
{
	public abstract class Movement : ScriptableObject
	{
		public abstract Task<Vector3> GetNextPositionAsync(Pawn pawn);
	}
}