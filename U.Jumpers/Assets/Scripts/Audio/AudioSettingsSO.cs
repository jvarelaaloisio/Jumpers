using UnityEngine;
using UnityEngine.Audio;

namespace Audio
{
	[CreateAssetMenu(menuName = "Models/Audio/Audio Settings", fileName = "AudioSettings")]
	public class AudioSettingsSO : ScriptableObject
	{
		[SerializeField] private bool loop;

		[Space, Header("Sound properties")]
		[SerializeField] private AudioMixerGroup outPutMixerGroup;

		[SerializeField] private PriorityLevel priority = PriorityLevel.Standard;
		[SerializeField, Range(0f, 1f)] private float volume = 1f;
		[SerializeField, Range(-3f, 3f)] private float pitch = 1f;
		[SerializeField, Range(-1f, 1f)] private float panStereo = 0f;
		[SerializeField, Range(0f, 1.1f)] private float reverbZoneMix = 1f;

		[Space, Header("Spatial Settings")]
		[SerializeField, Range(0f, 1f)] public float spatialBlend = 1f;

		[SerializeField] private AudioRolloffMode rollOffMode = AudioRolloffMode.Logarithmic;
		[SerializeField, Range(0.1f, 5f)] private float minDistance = 0.1f;
		[SerializeField, Range(5f, 100f)] private float maxDistance = 50f;
		[SerializeField, Range(0, 360)] private int spread = 0;
		[SerializeField, Range(0f, 5f)] private float dopplerLevel = 1f;

		[Space, Header("Ignores")]
		[SerializeField] private bool bypassEffects;

		[SerializeField] private bool bypassListenerEffects;
		[SerializeField] private bool bypassReverbZones;
		[SerializeField] private bool ignoreListenerVolume;
		[SerializeField] private bool ignoreListenerPause;

		public int Priority => (int) priority;
		public AudioMixerGroup OutPutMixerGroup => outPutMixerGroup;
		public bool Loop => loop;

		private enum PriorityLevel
		{
			Highest = 0,
			High = 64,
			Standard = 128,
			Low = 194,
			VeryLow = 256,
		}

		public void ApplyTo(AudioSource audioSource)
		{
			audioSource.loop = this.Loop;
			audioSource.outputAudioMixerGroup = this.outPutMixerGroup;
			audioSource.bypassEffects = this.bypassEffects;
			audioSource.bypassListenerEffects = this.bypassListenerEffects;
			audioSource.bypassReverbZones = this.bypassReverbZones;
			audioSource.priority = this.Priority;
			audioSource.volume = this.volume;
			audioSource.pitch = this.pitch;
			audioSource.panStereo = this.panStereo;
			audioSource.spatialBlend = this.spatialBlend;
			audioSource.reverbZoneMix = this.reverbZoneMix;
			audioSource.dopplerLevel = this.dopplerLevel;
			audioSource.spread = this.spread;
			audioSource.rolloffMode = this.rollOffMode;
			audioSource.minDistance = this.minDistance;
			audioSource.maxDistance = this.maxDistance;
			audioSource.ignoreListenerVolume = this.ignoreListenerVolume;
			audioSource.ignoreListenerPause = this.ignoreListenerPause;
		}
	}
}