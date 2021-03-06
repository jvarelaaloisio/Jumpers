using UnityEngine;

namespace Characters
{
	[CreateAssetMenu(menuName = "Models/CharacterModel", fileName = "CharacterModel")]
	public class AbilityUserModel : ScriptableObject
	{
		[SerializeField] private int[] manaLevels;
		[SerializeField] private float manaRegenerationPeriod;
		public int[] ManaLevels => manaLevels;
		public float ManaRegenerationPeriod => manaRegenerationPeriod;
	}
}