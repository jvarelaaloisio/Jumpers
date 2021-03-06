using Events.Channels;
using UnityEngine;

namespace Audio.Events
{
	[CreateAssetMenu(menuName = "Event Channels/Project Data Channels/Audio", fileName = "AudioChannel")]
	public class AudioChannelSo : ChannelSo<AudioClip, AudioSettingsSO, Vector3> { }
}
