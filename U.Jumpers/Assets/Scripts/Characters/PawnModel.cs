using System;
using UnityEngine;

namespace Characters
{
	[CreateAssetMenu(menuName = "Models/Characters/Pawn", fileName = "PawnModel")]
	public class PawnModel : ScriptableObject, IPawnModel
	{
		[SerializeField] private float moveDistance;
		[SerializeField] private float jumpDuration;
		[SerializeField] private float timeBetweenJumps;
		[SerializeField] private int lifePoints;
		[SerializeField] private int startingLifePoints;

		public float MoveDistance => moveDistance;
		public float JumpDuration => jumpDuration;
		public float TimeBetweenJumps => timeBetweenJumps;
		public int LifePoints => lifePoints;
		public int StartingLifePoints => startingLifePoints;
	}
}