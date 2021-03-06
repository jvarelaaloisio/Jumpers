using Events.UnityEvents;
using UnityEngine;
using CharacterController = Characters.CharacterController;

namespace Abilities
{
	public abstract class AbilitySo : ScriptableObject
	{
		[Header("Events Raised")]
		public IntUnityEvent onUse;
		[SerializeField] private Sprite icon;
		[SerializeField] protected int id;
		[SerializeField] protected float coolDown;

		public Sprite Icon => icon;

		public int ID => id;

		public float CoolDown => coolDown;

		public abstract bool CanBeUsed(CharacterController controller);

		public virtual void Use(CharacterController controller)
		{
			onUse.Invoke(id);
		}
	}
}