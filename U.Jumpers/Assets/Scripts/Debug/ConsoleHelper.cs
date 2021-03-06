using Events.Channels;
using Events.UnityEvents;
using UnityEngine;

namespace Debug
{
	public class ConsoleHelper : MonoBehaviour
	{
		public StringUnityEvent onWrite;
		[SerializeField] private StringChannelSo writeToConsoleChannel;

		private void Awake()
		{
			writeToConsoleChannel.Subscribe(onWrite.Invoke);
		}
	}
}
