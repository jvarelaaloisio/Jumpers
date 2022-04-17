using UnityEngine;

namespace Characters
{
	[CreateAssetMenu(menuName = "Models/Damage", fileName = "DamageModel")]
	public class DamageModel : ScriptableObject
	{
		[SerializeField] private int damage;

		public int Damage => damage;
	}
}