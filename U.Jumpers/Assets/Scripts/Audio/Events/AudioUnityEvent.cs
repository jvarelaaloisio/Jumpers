using UnityEngine;
using UnityEngine.Events;

namespace Audio.Events
{
	[System.Serializable]
	public class AudioUnityEvent : UnityEvent<AudioClip, AudioSettingsSO, Vector3> { }
}