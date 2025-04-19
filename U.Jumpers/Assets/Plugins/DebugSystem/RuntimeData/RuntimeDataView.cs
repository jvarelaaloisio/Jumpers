using System;
using DS.DataRetriever;
using TMPro;
using UnityEngine;

namespace Plugins.DebugSystem.GeneralInformation
{
	public class RuntimeDataView : MonoBehaviour
	{
		[SerializeField] private float frequency;
		[SerializeField] private TextMeshProUGUI runtimePanel;
		private float _actualTime;
		private IRetriever<string> _dataRetriever;
		public bool IsRetrieving { get; private set; }

		private void Start()
		{
			_dataRetriever = new StringRetriever(str => str + "\n");
			StartRetrieving();
		}

		private void Update()
		{
			if (!IsRetrieving || _dataRetriever.FetchSources is null)
				return;
			_actualTime += Time.deltaTime;
			if (!(_actualTime >= 1 / frequency))
				return;
			_actualTime = 0;
			UpdateText();
		}

		private void UpdateText()
		{
			bool dataWasRetrievedCorrectly = _dataRetriever.TryFetch(out var information);
			if (!dataWasRetrievedCorrectly)
				information = string.Empty;

			runtimePanel?.SetText(information);
		}

		public void AddRetriever(Func<string> getInformation)
			=> _dataRetriever.AddSource(getInformation);

		public void RemoveRetriever(Func<string> getInformation)
		{
			_dataRetriever.RemoveSource(getInformation);
			UpdateText();
		}

		public void StartRetrieving()
		{
			_actualTime = 0;
			IsRetrieving = true;
		}

		public void StopRetrieving() => IsRetrieving = false;
	}
}