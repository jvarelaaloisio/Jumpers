using Packages.UpdateManagement;
using UnityEngine;

namespace Audio
{
	public class AudioPlayer : MonoBehaviour
	{
		public AudioSource source;
		public bool isFree;

		public void PlayClip(AudioClip clip, AudioSettingsSO settings, Vector3 position)
		{
			gameObject.SetActive(true);
			transform.position = position;
			settings.ApplyTo(source);
			source.clip = clip;
			source.Play();
			isFree = false;
			if(settings.Loop)
				return;
			new CountDownTimer(clip.length, () =>
			{
				isFree = true;
				gameObject.SetActive(false);
			}).StartTimer();
		}
	}

	public static class AudioPlayerHelper
	{
		public static AudioPlayer With(this AudioPlayer current, AudioSource source, bool isFree = true)
		{
			current.source = source;
			current.isFree = isFree;
			return current;
		}
	}
}