using System;
using System.Collections.Generic;
using System.Linq;
using Audio.Events;
using Debug;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Audio
{
	public class AudioManager : MonoBehaviour
	{
		private const string MUSIC_PREFIX = "Music";
		private const string SFX_PREFIX = "Sfx";

		[Header("Setup")]
		[SerializeField] private int musicSrcQuantity;

		[SerializeField] private int sfxSrcQuantity;

		[Space, Header("Channels Listened")]
		[SerializeField] private AudioChannelSo playMusic;

		[SerializeField] private AudioChannelSo playSfx;

		private AudioPlayer[] musicPlayers;
		private AudioPlayer[] sfxPlayers;

		public int myScene;

		private void Awake()
		{
			var musicSourcesParent = new GameObject("Music Sources").transform;
			var sfxSourcesParent = new GameObject("Sfx Sources").transform;
			SceneManager.MoveGameObjectToScene(musicSourcesParent.gameObject,
				SceneManager.GetSceneByBuildIndex(myScene));
			SceneManager.MoveGameObjectToScene(sfxSourcesParent.gameObject, SceneManager.GetSceneByBuildIndex(myScene));
			musicPlayers = new AudioPlayer[musicSrcQuantity];
			sfxPlayers = new AudioPlayer[sfxSrcQuantity];
			AudioPlayer newPlayer;
			for (var i = 0; i < musicSrcQuantity; i++)
			{
				newPlayer = InstantiateAudioPlayer(MUSIC_PREFIX, i, musicSourcesParent);

				musicPlayers[i] = newPlayer;
			}

			for (var i = 0; i < sfxSrcQuantity; i++)
			{
				newPlayer = InstantiateAudioPlayer(SFX_PREFIX, i, sfxSourcesParent);
				sfxPlayers[i] = newPlayer;
			}

			playMusic.Subscribe(PlayMusic);
			playSfx.Subscribe(PlaySfx);
		}

		private void PlayMusic(AudioClip clip, AudioSettingsSO settings, Vector3 position)
		{
			if (!clip || !TryGetFreeAudioPlayer(musicPlayers, out var player))
			{
				Printer.Log("No music players are available");
				return;
			}

			player.PlayClip(clip, settings, position);
			Printer.Log($"{name.Colored("yellow")}: Music: {clip.name.Bold()} playing in {player.name.Bold()}");
		}

		private void PlaySfx(AudioClip clip, AudioSettingsSO settings, Vector3 position)
		{
			if (!clip || !TryGetFreeAudioPlayer(sfxPlayers, out var player))
				return;
			player.PlayClip(clip, settings, position);
			Printer.Log($"{name.Colored("yellow")}: sfx: {clip.name.Bold()} playing in {player.name.Bold()}");
		}

		private static bool TryGetFreeAudioPlayer(AudioPlayer[] players, out AudioPlayer player)
		{
			player = players.First(aP => aP.isFree);
			return players.Any(aP => aP.isFree);
		}

		// private static void PlayClip(AudioClip clip, AudioSettingsSO settings, Vector3 position, AudioSource source)
		// {
		// 	source.transform.position = position;
		// 	settings.ApplyTo(source);
		// 	source.clip = clip;
		// 	source.gameObject.SetActive(true);
		// 	source.Play();
		// }

		private static AudioPlayer InstantiateAudioPlayer(string playerPrefix, int id, Transform parent = null)
		{
			var newGO = new GameObject(playerPrefix + " Player " + id);
			newGO.gameObject.SetActive(false);
			newGO.transform.SetParent(parent);
			var source = newGO.AddComponent<AudioSource>();
			source.playOnAwake = false;
			var player = newGO.AddComponent<AudioPlayer>().With(source);
			return player;
		}

		private void OnDestroy()
		{
			playMusic.Unsubscribe(PlayMusic);
			playSfx.Unsubscribe(PlaySfx);
		}
	}
}