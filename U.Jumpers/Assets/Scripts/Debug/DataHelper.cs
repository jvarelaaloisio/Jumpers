using Events.Channels;
using Plugins.DebugSystem.GeneralInformation;
using UnityEngine;

namespace Debug
{
	public class DataHelper : MonoBehaviour
	{
		[SerializeField] private FuncStringChannel generalInfoChannel;
		[SerializeField] private GeneralInformationView generalInformationView;
		private void Awake()
		{
			generalInfoChannel.Subscribe(generalInformationView.AddInformation);
		}
	}
}
