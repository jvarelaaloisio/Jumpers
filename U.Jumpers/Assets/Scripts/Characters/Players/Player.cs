using Abilities;
using Characters.Abilities.Events;
using Events.Channels;
using Events.UnityEvents;
using UnityEngine;

namespace Characters.Players
{
	[SelectionBase]
	public class Player : CharacterView
	{
		[Space, Header("Channels Listened")]
		[SerializeField, Tooltip("Not Null")] private Vector3ChannelSo moveToPillar;
		[SerializeField, Tooltip("Can Be Null")] private VoidChannelSo activatePlayerMovement;
		[SerializeField, Tooltip("Not Null")] private AbilityChannelSo useAbility;

		[Space, Header("Events Raised")] public TransformArrayUnityEvent onCanMove;
		private Vector3 _lastCheckPoint;
		protected override void Awake()
		{
			base.Awake();
			Controller.OnFinishedMoving += ActivateMovementInput;
			moveToPillar.Subscribe(Controller.MoveCharacter);
			activatePlayerMovement.SubscribeSafely(ActivateMovementInput);
			useAbility.Subscribe(UseAbility);
		}

		protected override void Start()
		{
			base.Start();
			ActivateMovementInput();
		}

		private void UseAbility(AbilitySo ability)
		{
			if (!ability.CanBeUsed(Controller))
				return;
			ability.Use(Controller);
		}

		private void ActivateMovementInput()
		{
			Transform[] pillars = Controller.GetAvailablePillars();
			onCanMove?.Invoke(pillars);
		}

		protected override void OnDeath()
		{
			base.OnDeath();
			transform.position = _lastCheckPoint;
			Controller.Damageable.TakeDamage(-model.LifePoints);
		}

		public void SetCheckPoint(Transform checkPoint)
		{
			var temp = checkPoint.position;
			temp.y = transform.position.y;
			_lastCheckPoint = temp;
		}

		private void OnDestroy()
		{
			moveToPillar.Unsubscribe(Controller.MoveCharacter);
			activatePlayerMovement.UnSubscribe(ActivateMovementInput);
			useAbility.Unsubscribe(UseAbility);
		}
	}
}