using Audio;
using Audio.Events;
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
		[SerializeField] protected CharacterModel model;
		[SerializeField] protected float rotationDuration;

		[Space, Header("Audio")]
		[SerializeField] private AudioSettingsSO audioSettings;
		[SerializeField] private AudioClip onMoveClip;
		[SerializeField] private AudioClip onDamageClip;
		[SerializeField] private AudioClip onDeathClip;

		[Space, Header("Events Raised")] public UnityEvent onMove;
		public UnityEvent onMoveFinished;
		public IntUnityEvent onTakeDamage;
		[SerializeField] private bool onLifeChangedIsLerp;
		public FloatUnityEvent onLifeChanged;
		public UnityEvent onDeath;
		public AudioUnityEvent onAudio;

		[Space, Header("Debug"), Tooltip("Can Be Null")]
		[SerializeField] private FuncStringChannel generalInfoChannel;

		protected CharacterController Controller;
		public CharacterModel Model => model;

		protected virtual void Awake()
		{
			Controller = new CharacterController(Model, transform,
				() => transform.position, rotationDuration, gameObject.scene.buildIndex);
			Controller.OnCharacterMoves += MoveCharacter;
			Controller.OnFinishedMoving += onMoveFinished.Invoke;
			Controller.Damageable.OnDeath += OnDeath;
		}

		protected virtual void Start()
		{
			Printer.Log(
				LogLevel.Log,
				$"{name} created with <color=green>{Controller.Damageable.LifePoints}</color> LP");
			generalInfoChannel.RaiseEventSafely(
				() => $"{name} <color=green>{Controller.Damageable.LifePoints}</color> LP");
		}

		protected virtual void OnDeath()
		{
			Printer.Log($"{name.Bold()} died");
			onAudio.Invoke(onDeathClip, audioSettings, transform.position);
			onDeath.Invoke();
		}

		protected virtual void MoveCharacter(Vector3 destination)
		{
			onAudio.Invoke(onMoveClip, audioSettings, transform.position);
			onMove.Invoke();
		}

		public void TakeDamage(int damage)
		{
			Damageable damageable = Controller.Damageable;
			damageable.TakeDamage(damage);
			onTakeDamage.Invoke(damage);
			onLifeChanged.Invoke(onLifeChangedIsLerp
				? (float) damageable.LifePoints / damageable.MaxLifePoints
				: damageable.LifePoints);
			onAudio.Invoke(onDamageClip, audioSettings, transform.position);
			Printer.Log(
				$"{name} took <color=red>{damage}</color> damage and now has <color=green>{Controller.Damageable.LifePoints}/{Controller.Damageable.MaxLifePoints}</color>");
		}
	}
}