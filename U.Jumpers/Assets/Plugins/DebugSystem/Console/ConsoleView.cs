using System.Collections.Generic;
using DS.DebugConsole;
using UnityEngine;
using UnityEngine.UI;

namespace Plugins.DebugSystem.Console
{
	public class ConsoleView : MonoBehaviour
	{
		[SerializeField] private Text consoleBody;
		[SerializeField] private InputField inputField;
		[SerializeField] private ConsoleControllerSO consoleController;
		private IDebugConsole<string> debugConsole;
		private List<ICommand<string>> commands;

		public void SubmitInput(string input)
		{
			if (!Input.GetButtonDown("Submit"))
				return;
			ReadInput(input);
		}

		public void SubmitInput(Text text)
		{
			ReadInput(text.text);
		}

		private void ReadInput(string input)
		{
			if (input == string.Empty)
				return;
			_ = consoleController.TryUseInput(input);
			inputField.text = string.Empty;
		}

		private void Start()
		{
			consoleController.onFeedback += WriteFeedback;
		}

		public void WriteFeedback(string newFeedBack)
		{
			consoleBody.text += "\n" + newFeedBack;
			inputField.ActivateInputField();
		}
	}
}