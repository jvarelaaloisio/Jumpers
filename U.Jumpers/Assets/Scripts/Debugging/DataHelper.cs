using Events.Channels;
using Plugins.DebugSystem.GeneralInformation;
using UnityEngine;

namespace Debugging
{
	public class DataHelper : MonoBehaviour
	{
		[SerializeField] private FuncStringChannel generalInfoChannel;
		[SerializeField] private RuntimeDataView runtimeDataView;
		private void Awake()
		{
			if (runtimeDataView == null)
			{
				Debug.LogError($"{name}: {nameof(runtimeDataView)} is null!");
				return;
			}
			generalInfoChannel?.Subscribe(runtimeDataView.AddRetriever);
		}
	}
}
