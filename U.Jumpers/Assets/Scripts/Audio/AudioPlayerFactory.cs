using System;
using Core.Factory;
using Debugging;
using UnityEngine;

namespace Audio
{
	/// <summary>
	/// This class is obsolete.
	/// Use AudioPlayer.Factory instead.
	/// </summary>
	[Obsolete]
	public class AudioPlayerFactory : UnityFactory<AudioPlayer>
	{
		public AudioPlayerFactory(
			string namePrefix,
			Transform parent = null)
			: base(namePrefix,
					" Player ",
					parent)
		{ }

		protected override AudioPlayer InstantiateObject()
		{
			int id = GetNextID();
			Printer.Log(LogLevel.Log, "instantiated id: " + id);
			var newGO = new GameObject(GetName(id));
			newGO.gameObject.SetActive(false);
			newGO.transform.SetParent(Parent);
			var source = newGO.AddComponent<AudioSource>();
			source.playOnAwake = false;
			var player = newGO.AddComponent<AudioPlayer>().With(source);
			player.OnAudioFinished = Dispose;
			return player;
		}

		protected override void EnableObject(AudioPlayer player)
			=> player.gameObject.SetActive(true);

		protected override void DisposeObject(AudioPlayer player)
			=> player.gameObject.SetActive(false);
	}
}