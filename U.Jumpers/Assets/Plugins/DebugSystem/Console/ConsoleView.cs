using System.Collections.Generic;
using System.Linq;
using DS.DebugConsole;
using UnityEngine;
using UnityEngine.UI;

namespace Plugins.DebugSystem.Console
{
	public class ConsoleView : MonoBehaviour
	{
		private const int CHARACTER_LIMIT = 13000;
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
			if (consoleBody.text.Length >= CHARACTER_LIMIT)
			{ 
				var newBody = consoleBody.text.Substring(consoleBody.text.IndexOf('\n') + 1);
				consoleBody.text = newBody;
			}
			inputField.ActivateInputField();
		}
	}
}