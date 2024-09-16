using System;
using System.Collections.Generic;
using System.Linq;
using DS.DebugConsole;
using Plugins.DebugSystem.Console.Commands;
using UnityEngine;

namespace Plugins.DebugSystem.Console
{
	[CreateAssetMenu(menuName = "Debug Console/Debug Console", fileName = "DebugConsole", order = 1000)]
	public class ConsoleWrapper : ScriptableObject
	{
		[SerializeField] protected List<CommandSO> commands;
		[SerializeField] protected char[] separators;
		protected IDebugConsole<string> DebugConsole;
		public Action<string> onFeedback;
		
		protected void OnEnable()
		{
			var commandList = new List<ICommand<string>>(this.commands);
			DebugConsole = new SimpleDebugConsole<string>((str)=> onFeedback(str), commandList);
			var helpCommand = new HelpCommand(DebugConsole);
			DebugConsole.AddCommand(helpCommand);
		}

		public bool TryUseInput(string input)
		{
			string[] inputs = input.Split(separators);
			string commandName = inputs[0];
			if (!DebugConsole.IsValidCommand(commandName))
			{
				onFeedback($"command <color=red>{commandName}</color> not recognized");
				return false;
			}

			int argsQty = inputs.Length - 1;
			string[] args = argsQty >= 0 ? inputs.ToList().GetRange(1, inputs.Length - 1).ToArray() : new string[0];

			DebugConsole.ExecuteCommand(commandName, args);
			return true;
		}
	}
}