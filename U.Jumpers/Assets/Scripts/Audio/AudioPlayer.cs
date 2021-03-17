using UnityEngine;
using VarelaAloisio.UpdateManagement.Runtime;

namespace Audio
{
	public class AudioPlayer : MonoBehaviour
	{
		public AudioSource Source { get; set; }
		public bool IsFree { get; set; }
		public int SceneIndex { get; set; }

		public void PlayClip(AudioClip clip, AudioSettingsSO settings, Vector3 position)
		{
			gameObject.SetActive(true);
			transform.position = position;
			settings.ApplyTo(Source);
			Source.clip = clip;
			Source.Play();
			IsFree = false;
			if (settings.Loop)
				return;
			new CountDownTimer(clip.length, () =>
				{
					IsFree = true;
					gameObject.SetActive(false);
				},
				SceneIndex).StartTimer();
		}
	}

	public static class AudioPlayerHelper
	{
		public static AudioPlayer With(this AudioPlayer current, AudioSource source, bool isFree = true)
		{
			current.Source = source;
			current.IsFree = isFree;
			return current;
		}

		public static AudioPlayer InScene(this AudioPlayer current, int sceneIndex)
		{
			current.SceneIndex = sceneIndex;
			return current;
		}
	}
}