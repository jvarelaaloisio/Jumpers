using Characters;
using UnityEngine;

namespace Abilities
{
	[CreateAssetMenu(menuName = "Models/Abilities/Fire", fileName = "FireAbility")]
	class FireAbility : AbilitySo
	{
		[SerializeField] private int range;
		[SerializeField] private int damage;
		[SerializeField] private LayerMask enemyMask;

		public override bool CanBeUsed(Pawn controller)
		{
			return true;
		}

		public override void Use(Pawn controller)
		{
			var ray = new Ray(controller.GetTransform.position, controller.GetTransform.right);
			if (Physics.Raycast(ray, out var hit, range, enemyMask, QueryTriggerInteraction.Collide))
				hit.transform.SendMessage("TakeDamage", damage);
			ray.direction *= -1;
			if (Physics.Raycast(ray, out hit, range, enemyMask, QueryTriggerInteraction.Collide))
				hit.transform.SendMessage("TakeDamage", damage);
			base.Use(controller);
		}
	}
}