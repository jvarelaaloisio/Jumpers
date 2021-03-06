using UnityEngine;

namespace Characters.Enemies
{
	[CreateAssetMenu(menuName = "Models/Damage", fileName = "DamageModel")]
	public class DamageModel : ScriptableObject
	{
		[SerializeField] private int damage;

		public int Damage => damage;
	}
}