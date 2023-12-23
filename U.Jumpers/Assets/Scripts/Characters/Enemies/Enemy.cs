using System;
using System.Linq;
using Characters.Movements;
using Core.Factory;
using Debugging;
using UnityEngine;
using VarelaAloisio.UpdateManagement.Runtime;

namespace Characters.Enemies
{
	public class Enemy : CharacterView
	{
		private const string TakeDamageMessage = "TakeDamage";

		[SerializeField]
		private DamageModel damageModel;

		[SerializeField]
		public Movement movement;

		[SerializeField]
		private GameObject telegrapher;

		private CountDownTimer _move;
		private Vector3 _nextPosition;

		protected override void Awake()
		{
			base.Awake();
			_move = new CountDownTimer(Model.TimeBetweenJumps, Move, gameObject.scene.buildIndex);
		}

		protected virtual void OnEnable()
		{
			pawn.OnFinishedMoving += PlanNextMovement;
		}

		protected override void Start()
		{
			base.Start();
			PlanNextMovement();
		}

		protected virtual void OnDisable()
		{
			_move?.StopTimer();
			pawn.OnFinishedMoving -= PlanNextMovement;
		}

		protected virtual void PlanNextMovement()
		{
			_nextPosition = movement.GetNextPositionAsync(pawn).Result;
			telegrapher.transform.rotation = Quaternion.LookRotation(_nextPosition - transform.position);
			_move?.StartTimer();
		}

		protected virtual void Move()
		{
			pawn.MoveCharacter(_nextPosition);
		}

		private void OnTriggerEnter(Collider other)
		{
			other.SendMessage(TakeDamageMessage, damageModel.Damage);
		}

		public class Builder
		{
			private readonly Enemy _enemy;

			public Builder(Enemy enemy)
				=> _enemy = enemy;

			public Builder SetModel(PawnModel model)
			{
				_enemy.model = model;
				return this;
			}
			public Builder SetMovement(Movement movement)
			{
				_enemy.movement = movement;
				return this;
			}
			public Builder SetDamage(DamageModel damageModel)
			{
				_enemy.damageModel = damageModel;
				return this;
			}
			public Builder SetMesh(Mesh mesh)
			{
				_enemy.GetComponent<MeshFilter>().sharedMesh = mesh;
				return this;
			}
			public Builder SetMaterials(Material[] materials)
			{
				_enemy.GetComponent<MeshRenderer>().sharedMaterials = materials;
				return this;
			}
			public Builder SetOnMoveClip(AudioClip onMove)
			{
				_enemy.onMoveClip = onMove;
				return this;
			}
			public Builder SetOnDamageClip(AudioClip onDamage)
			{
				_enemy.onDamageClip = onDamage;
				return this;
			}
			public Builder SetOnDeathClip(AudioClip onDeath)
			{
				_enemy.onDeathClip = onDeath;
				return this;
			}

			public Enemy Build()
				=> _enemy;
		}
		
		public class Factory : UnityFactory<Enemy>
		{
			private readonly string _namePrefix;
			private readonly GameObject _prefab;

			private int _lastId;

			public Factory(
				string namePrefix,
				Transform parent = null,
				GameObject prefab = null)
				: base(namePrefix,
						" Enemy ",
						parent)
			{
				_prefab = prefab ? prefab : new GameObject();
			}

			protected override Enemy InstantiateObject()
			{
				int id = GetNextID();
				Printer.Log(LogLevel.Log, "instantiated id: " + id);
				var newGO = Instantiate(_prefab, Parent);
				// new GameObject(GetName(id));
				newGO.name = GetName(id);
				newGO.gameObject.SetActive(false);
				// newGO.transform.SetParent(Parent);
				var source = newGO.AddComponent<AudioSource>();
				source.playOnAwake = false;
				var enemy = newGO.AddComponent<Enemy>();
				enemy.onDeath.AddListener(c => DisposeObject((Enemy) c));
				return enemy;
			}

			protected override void EnableObject(Enemy player)
			{
				throw new System.NotImplementedException();
			}

			protected override void DisposeObject(Enemy player)
			{
				throw new System.NotImplementedException();
			}
		}
	}
}