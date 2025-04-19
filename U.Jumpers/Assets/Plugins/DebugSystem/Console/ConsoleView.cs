using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Plugins.DebugSystem.Console
{
	public class ConsoleView : MonoBehaviour
	{
		private const int CHARACTER_LIMIT = 13000;
		[SerializeField] private TextMeshProUGUI consoleBody;
		[SerializeField] private TMP_InputField inputField;
		[SerializeField] private Button submit;
		[SerializeField] private ConsoleWrapper consoleWrapper;

		private void OnEnable()
		{
			submit?.onClick.AddListener(HandleSubmitClick);
			inputField?.onSubmit.AddListener(SubmitInput);
			if (consoleWrapper)
				consoleWrapper.log += WriteToOutput;
		}
		
		private void OnDisable()
		{
			submit?.onClick.RemoveListener(HandleSubmitClick);
			inputField?.onSubmit.RemoveListener(SubmitInput);
			if (consoleWrapper)
				consoleWrapper.log -= WriteToOutput;
		}

		private void HandleSubmitClick() => SubmitInput(inputField.text);

		private void SubmitInput(string input)
		{
			if (input == string.Empty)
				return;
			if (consoleWrapper == null)
			{
				Debug.LogError($"{name}: {nameof(consoleWrapper)} is null!");
				return;
			}
			_ = consoleWrapper.TryUseInput(input);
			inputField?.SetTextWithoutNotify(string.Empty);
		}

		public void WriteToOutput(string newFeedBack)
		{
			if (!consoleBody)
			{
				Debug.LogError($"{name}: {nameof(consoleBody)} is null!");
				return;
			}
			consoleBody.text += "\n" + newFeedBack;
			var watchdog = 10;
			var bodyText = consoleBody.text;
			while (watchdog-- > 0 && bodyText.Length >= CHARACTER_LIMIT)
			{ 
				var newBody = bodyText[(bodyText.IndexOf('\n') + 1)..];
				consoleBody.text = newBody;
			}
			inputField?.ActivateInputField();
		}
	}
}