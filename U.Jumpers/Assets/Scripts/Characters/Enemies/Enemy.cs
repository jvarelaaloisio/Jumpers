using System.Linq;
using Characters.Movements;
using Debugging;
using UnityEngine;
using VarelaAloisio.UpdateManagement.Runtime;

namespace Characters.Enemies
{
	public class Enemy : CharacterView
	{
		private const string TAKE_DAMAGE_MESSAGE = "TakeDamage";
		[SerializeField] private DamageModel damageModel;
		public Movement movement;
		[SerializeField] private GameObject telegrapher;
		private CountDownTimer _move;
		private Vector3 _nextPosition;

		protected override void Awake()
		{
			base.Awake();
			_move = new CountDownTimer(Model.TimeBetweenJumps, Move, gameObject.scene.buildIndex);
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
				Printer.Log(name + ": can't move!");
				return;
			}

			_nextPosition = movement.GetNextPosition(transform, pillars.Select(t => t.position).ToArray());
			telegrapher.transform.rotation = Quaternion.LookRotation(_nextPosition - transform.position);
			_move.StartTimer();
		}

		protected virtual void Move()
		{
			Controller.MoveCharacter(_nextPosition);
		}

		private void OnTriggerEnter(Collider other)
		{
			other.SendMessage(TAKE_DAMAGE_MESSAGE, damageModel.Damage);
		}
	}
}