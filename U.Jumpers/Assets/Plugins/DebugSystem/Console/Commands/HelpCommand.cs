using System;
using System.Collections.Generic;
using System.Linq;
using DS.DebugConsole;

namespace Plugins.DebugSystem.Console.Commands
{
	public class HelpCommand : ICommand<string>
	{
		private readonly IDebugConsole<string> _console;

		public HelpCommand(IDebugConsole<string> console) => _console = console;

		public string Name => "help";

		public IEnumerable<string> Aliases => new [] {"h", "Help", "HELP", "?", "h?", "help?", "help!"};

		public string Description => "Shows all commands information";

		public void Execute(Action<string> log, params string[] args)
		{
			const string baseOutput = "<color=green>name\t\t|\t\tdescription\n</color>";
			var commandsInfo = _console
			                   .Commands
			                   .Aggregate(baseOutput,
			                              (current, command) => current + $"{command.Name}\t\t|\t\t{command.Description}\n");
			log?.Invoke(commandsInfo);
		}
	}
}