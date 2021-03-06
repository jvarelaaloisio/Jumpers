using UnityEngine;
using UnityEngine.Events;

namespace Managers
{
	public class SceneInitializer : MonoBehaviour
	{
		[SerializeField] private UnityEvent initializationEvent;
		private void Awake()
		{
			initializationEvent?.Invoke();
		}
	}
}