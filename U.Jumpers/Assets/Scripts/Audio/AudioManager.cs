using Audio.Events;
using Core.Factory;
using Debugging;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Audio
{
	public class AudioManager : MonoBehaviour
	{
		private const string MusicPrefIx = "Music";
		private const string SfxPrefix = "Sfx";

		[Header("Setup")]
		[SerializeField] private int musicSrcQuantity;

		[SerializeField] private bool isMusicPoolDynamic;
		[SerializeField] private int sfxSrcQuantity;
		[SerializeField] private bool isSfxPoolDynamic;

		[Space, Header("Channels Listened")]
		[SerializeField] private AudioChannelSo playMusic;

		[SerializeField] private AudioChannelSo playSfx;

		private UnityFactory<AudioPlayer> _musicPlayerFactory;
		private UnityFactory<AudioPlayer> _sfxPlayerFactory;

		private void Awake()
		{
			var musicSourcesParent = new GameObject("Music Sources").transform;
			var sfxSourcesParent = new GameObject("Sfx Sources").transform;
			SceneManager.MoveGameObjectToScene(musicSourcesParent.gameObject, gameObject.scene);
			SceneManager.MoveGameObjectToScene(sfxSourcesParent.gameObject, gameObject.scene);
			_musicPlayerFactory = new AudioPlayer.Factory(MusicPrefIx, musicSourcesParent);
			_musicPlayerFactory.InitializePool(musicSrcQuantity, isMusicPoolDynamic);
			_sfxPlayerFactory = new AudioPlayer.Factory(SfxPrefix, sfxSourcesParent);
			_sfxPlayerFactory.InitializePool(sfxSrcQuantity, isSfxPoolDynamic);
			
			playMusic.Subscribe(PlayMusic);
			playSfx.Subscribe(PlaySfx);
		}

		private static void PlayAudio(AudioClip clip,
			AudioSettingsSO settings,
			Vector3 position,
			//TODO: Replace with Pool
			UnityFactory<AudioPlayer> audioPlayerFactory,
			string logPrefix)
		{
			if (!clip)
			{
				Printer.Log(LogLevel.Warning, $"{logPrefix}you're trying to play a null audio");
				return;
			}
			
			var player = audioPlayerFactory.Get();
			if (!player)
			{
				Printer.Log(LogLevel.Warning, $"{logPrefix}cannot play {clip.name.Bold()}");
				return;
			}

			player.PlayClip(clip, settings, position);
			Printer.Log(LogLevel.Info,
				$"{logPrefix}{clip.name.Bold()} playing in {player.name.Bold()}");
		}

		private void PlayMusic(AudioClip clip, AudioSettingsSO settings, Vector3 position)
			=> PlayAudio(clip, settings, position, _musicPlayerFactory, $"{name.Colored("orange")}: Music: ");

		private void PlaySfx(AudioClip clip, AudioSettingsSO settings, Vector3 position)
			=> PlayAudio(clip, settings, position, _sfxPlayerFactory, $"{name.Colored("yellow")}: sfx: ");

		private void OnDestroy()
		{
			playMusic.Unsubscribe(PlayMusic);
			playSfx.Unsubscribe(PlaySfx);
		}
	}
}