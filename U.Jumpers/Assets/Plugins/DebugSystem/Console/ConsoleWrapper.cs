using System;
using System.Collections.Generic;
using System.Linq;
using DS.DebugConsole;
using Plugins.DebugSystem.Console.Commands;
using Plugins.DebugSystem.Console.Extensions;
using UnityEngine;

namespace Plugins.DebugSystem.Console
{
	[CreateAssetMenu(menuName = "Debug Console/Debug Console", fileName = "DebugConsole", order = 1000)]
	public class ConsoleWrapper : ScriptableObject
	{
		[SerializeField] protected List<CommandSo> commands;
		[SerializeField] protected char[] separators;
		protected IDebugConsole<string> DebugConsole;
		public Action<string> log = delegate { };
		
		protected void OnEnable()
		{
			DebugConsole = new SimpleDebugConsole<string>((str)=> log(str), commands.Cast<ICommand<string>>().ToArray());
			var aliasesCommand = new AliasesCommand(DebugConsole);
			DebugConsole.AddCommand(aliasesCommand);
			var helpCommand = new HelpCommand(DebugConsole);
			DebugConsole.AddCommand(helpCommand);
		}

		public bool TryAddCommand(ICommand<string> command)
			=> DebugConsole.TryAddCommand(command);

		public bool TryUseInput(string input)
		{
			var inputs = input.Split(separators);
			var commandName = inputs[0];
			if (!DebugConsole.IsValidCommand(commandName))
			{
				log($"command not found: {commandName.Colored(Color.red)}");
				return false;
			}

			var args = inputs[1..];

			DebugConsole.ExecuteCommand(commandName, args);
			return true;
		}
	}
}