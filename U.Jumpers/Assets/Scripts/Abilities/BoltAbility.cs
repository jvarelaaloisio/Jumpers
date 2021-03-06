using UnityEngine;
using CharacterController = Characters.CharacterController;

namespace Abilities
{
	[CreateAssetMenu(menuName = "Models/Abilities/Bolt", fileName = "BoltAbility")]
	class BoltAbility : AbilitySo
	{
		[SerializeField] private int range;
		[SerializeField] private int damage;
		[SerializeField] private LayerMask enemyMask;

		public override bool CanBeUsed(CharacterController controller)
		{
			return true;
		}

		public override void Use(CharacterController controller)
		{
			Collider[] enemies = Physics.OverlapSphere(
				controller.GetTransform.position,
				range,
				enemyMask,
				QueryTriggerInteraction.Collide);
			if (enemies.Length <= 0)
				return;
			foreach (Collider enemy in enemies)
			{
				enemy.SendMessage("TakeDamage", damage);
			}
			base.Use(controller);
		}
	}
}