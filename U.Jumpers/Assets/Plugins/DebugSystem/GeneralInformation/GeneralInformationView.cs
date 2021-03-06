using System;
using DS.DataRetriever;
using UnityEngine;
using UnityEngine.UI;

namespace Plugins.DebugSystem.GeneralInformation
{
	public class GeneralInformationView : MonoBehaviour
	{
		[SerializeField] private float frequency;
		private float _actualTime;
		[SerializeField] private Text generalInformationText;
		private IDataRetriever<string> _dataRetriever;
		public bool IsUpdatingInfo { get; private set; }

		public void StopUpdatingInfo() => IsUpdatingInfo = false;

		public void AddInformation(Func<string> getInformation) => _dataRetriever.AddDataSource(getInformation);

		public void RemoveInformation(Func<string> getInformation)
		{
			_dataRetriever.RemoveDataSource(getInformation);
			UpdateText();
		}
	
		public void StartUpdatingInfo()
		{
			_actualTime = 0;
			IsUpdatingInfo = true;
		}

		private void Start()
		{
			_dataRetriever = new TextDataRetriever((str) => str + "\n");
			IsUpdatingInfo = true;
		}

		private void Update()
		{
			if (!IsUpdatingInfo || _dataRetriever.OnRetrieveData is null)
				return;
			_actualTime += Time.deltaTime;
			if (!(_actualTime >= 1 / frequency))
				return;
			_actualTime = 0;
			UpdateText();
		}

		private void UpdateText()
		{
			bool dataWasRetrievedCorrectly = _dataRetriever.TryRetrieveData(out var information);
			if (!dataWasRetrievedCorrectly)
				information = string.Empty;

			generalInformationText.text = information;
		}
	}
}