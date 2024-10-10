using System;
using Core.Factory;
using Debugging;
using UnityEngine;
using VarelaAloisio.UpdateManagement.Runtime;

namespace Audio
{
	public class AudioPlayer : MonoBehaviour
	{
		public AudioSource Source { get; set; }
		public Action<AudioPlayer> OnAudioFinished;
		private int _sceneIndex;

		private void Awake()
		{
			_sceneIndex = gameObject.scene.buildIndex;
		}

		public void PlayClip(AudioClip clip, AudioSettingsSO settings, Vector3 position)
		{
			transform.position = position;
			settings.ApplyTo(Source);
			Source.clip = clip;
			Source.Play();
			if (settings.Loop)
				return;
			new CountDownTimer(clip.length, () => OnAudioFinished(this), _sceneIndex).StartTimer();
		}
		
		public class Factory : UnityFactory<AudioPlayer>
		{
			private readonly Action<AudioPlayer> _handleAudioFinished;

			public Factory(
				string namePrefix, Transform parent = null)
				: base(namePrefix,
						" Player ",
						parent)
			{
				//TODO: receive parameter Action<AudioPlayer> handleAudioFinished
				_handleAudioFinished = Dispose;
			}

			protected override AudioPlayer Create()
			{
				int id = GetNextID();
				Printer.Log(LogLevel.Log, "instantiated id: " + id);
				var newGO = new GameObject(GetName(id));
				newGO.gameObject.SetActive(false);
				newGO.transform.SetParent(Parent);
				var source = newGO.AddComponent<AudioSource>();
				source.playOnAwake = false;
				var player = newGO.AddComponent<AudioPlayer>();
				player.Source = source;
				player.OnAudioFinished = _handleAudioFinished;
				return player;
			}

			protected override void EnableObject(AudioPlayer player)
				=> player.gameObject.SetActive(true);

			protected override void DisposeObject(AudioPlayer player)
				=> player.gameObject.SetActive(false);
		}
	}

	public static class AudioPlayerHelper
	{
		public static AudioPlayer With(this AudioPlayer current, AudioSource source)
		{
			current.Source = source;
			return current;
		}
	}
}