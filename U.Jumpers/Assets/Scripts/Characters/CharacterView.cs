using System;
using System.Collections.Generic;
using Audio;
using Audio.Events;
using Characters.Events;
using Core.Providers;
using Debugging;
using Events.Channels;
using Events.UnityEvents;
using LS;
using UnityEngine;
using UnityEngine.Events;

namespace Characters
{
	public class CharacterView : MonoBehaviour
	{
		[Header("Setup")]
		[SerializeField]
		protected PawnModel model;

		[SerializeField] private DataProvider<List<CharacterView>> charactersProvider;

		[SerializeField]
		protected float rotationDuration;

		[Space]
		[Header("Audio")]
		[SerializeField]
		private AudioSettingsSO audioSettings;

		[SerializeField]
		protected AudioClip onMoveClip;

		[SerializeField]
		protected AudioClip onDamageClip;

		[SerializeField]
		protected AudioClip onDeathClip;

		[Space]
		[Header("Events Raised")]
		public UnityEvent onMove;

		public UnityEvent onMoveFinished;
		public IntUnityEvent onTakeDamage;

		[SerializeField]
		private bool onLifeChangedIsLerp;

		public FloatUnityEvent onLifeChanged;
		public CharacterUnityEvent onDeath;
		public AudioUnityEvent onAudio;

		[Space]
		[Header("Debug")]
		[SerializeField]
		[Tooltip("Can Be Null")]
		private FuncStringChannel generalInfoChannel;

		public Pawn Pawn { get; set; }
		public PawnModel Model => model;

		protected virtual void Awake()
		{
			Pawn = new Pawn(Model, transform,
												() => transform.position, rotationDuration,
												gameObject.scene.buildIndex);
			Pawn.OnCharacterMoves += MoveCharacter;
			Pawn.OnFinishedMoving += onMoveFinished.Invoke;
			Pawn.Damageable.OnDeath += OnDeath;
		}

		protected virtual void Start()
		{
			Printer.Log(
						LogLevel.Log,
						$"{name} created with <color=green>{Pawn.Damageable.LifePoints}</color> LP");
			generalInfoChannel.RaiseEventSafely(
												() =>
													$"{name} <color=green>{Pawn.Damageable.LifePoints}</color> LP");
		}

		private void OnEnable()
		{
			charactersProvider.Value.Add(this);
		}

		private void OnDisable()
		{
			charactersProvider.Value.Remove(this);
		}

		protected virtual void OnDeath()
		{
			Printer.Log($"{name.Bold()} died");
			onAudio.Invoke(onDeathClip, audioSettings, transform.position);
			onDeath.Invoke(this);
		}

		protected virtual void MoveCharacter(Vector3 destination)
		{
			onAudio.Invoke(onMoveClip, audioSettings, transform.position);
			onMove.Invoke();
		}

		public void TakeDamage(int damage)
		{
			Damageable damageable = Pawn.Damageable;
			damageable.TakeDamage(damage);
			onTakeDamage.Invoke(damage);
			onLifeChanged.Invoke(onLifeChangedIsLerp
									? (float) damageable.LifePoints / damageable.MaxLifePoints
									: damageable.LifePoints);
			onAudio.Invoke(onDamageClip, audioSettings, transform.position);
			Printer.Log(
						$"{name} took <color=red>{damage}</color> damage and now has <color=green>{Pawn.Damageable.LifePoints}/{Pawn.Damageable.MaxLifePoints}</color>");
		}
	}
}