using System;
using Audio.Events;
using UnityEngine;

namespace Audio
{
	public class ClipStarter : MonoBehaviour
	{
		[SerializeField] private AudioSettingsSO settings;
		[SerializeField] private AudioClip clip;
		[SerializeField] private Vector3 position;
		[SerializeField] private AudioUnityEvent onStart;
		private void Start()
		{
			onStart.Invoke(clip, settings, position);
		}
	}
}