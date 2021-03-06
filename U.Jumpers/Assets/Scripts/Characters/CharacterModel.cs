using UnityEngine;

namespace Characters
{
	[CreateAssetMenu(menuName = "Models/CharacterModel", fileName = "CharacterModel")]
	public class CharacterModel : ScriptableObject, ICharacterModel
	{
		[SerializeField] private float moveDistance;
		[SerializeField] private float jumpDuration;
		[SerializeField] private float timeBetweenJumps;
		[SerializeField] private int lifePoints;
		public float MoveDistance => moveDistance;
		public float JumpDuration => jumpDuration;
		public float TimeBetweenJumps => timeBetweenJumps;
		public int LifePoints => lifePoints;
	}
}