using System.Linq;
using Characters.Movements;
using Packages.UpdateManagement;
using UnityEngine;
using LS;

namespace Characters.Enemies
{
	public class Enemy : CharacterView
	{
		private const string TAKE_DAMAGE_MESSAGE = "TakeDamage";
		[SerializeField] private DamageModel damageModel;
		[SerializeField] protected Movement movement;
		[SerializeField] private GameObject telegrapher;
		private CountDownTimer move;
		private Vector3 nextPosition;

		protected override void Awake()
		{
			base.Awake();
			move = new CountDownTimer(Model.TimeBetweenJumps, Move);
			Controller.OnFinishedMoving += PlanNextMovement;
		}

		protected override void Start()
		{
			base.Start();
			PlanNextMovement();
		}

		protected virtual void PlanNextMovement()
		{
			Transform[] pillars = Controller.GetAvailablePillars();
			if (pillars.Length.Equals(0))
			{
				Debug.Log(name + ": can't move!");
				return;
			}

			nextPosition = movement.GetNextPosition(transform, pillars.Select(t => t.position).ToArray());
			telegrapher.transform.rotation = Quaternion.LookRotation(nextPosition - transform.position);
			move.StartTimer();
		}

		protected virtual void Move()
		{
			Controller.MoveCharacter(nextPosition);
		}

		private void OnTriggerEnter(Collider other)
		{
			other.SendMessage(TAKE_DAMAGE_MESSAGE, damageModel.Damage);
		}
	}
}