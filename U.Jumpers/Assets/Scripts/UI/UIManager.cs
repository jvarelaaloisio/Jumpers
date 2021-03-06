using System;
using Events.Channels;
using Events.UnityEvents;
using UnityEngine;

namespace Managers
{
	public class UIManager : MonoBehaviour
	{
		[Header("Events Raised")]
		public FloatUnityEvent onPlayerLifeChanged;
		[SerializeField, Tooltip("Can Be Null")] private FloatChannelSo playerLifeChanged;
		private void Awake()
		{
			playerLifeChanged.SubscribeSafely(onPlayerLifeChanged.Invoke);
		}
	}
}
