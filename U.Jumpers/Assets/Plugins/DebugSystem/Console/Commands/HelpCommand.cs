using System;
using System.Collections.Generic;
using DS.DebugConsole;

namespace Plugins.DebugSystem.Console.Commands
{
	public class HelpCommand : ICommand<string>
	{
		private readonly IDebugConsole<string> _console;

		public HelpCommand(IDebugConsole<string> console)
		{
			_console = console;
			Aliases = new List<string> {"h", "Help"};
			Description = "Shows all commands information";
		}

		public void Execute(string[] args, Action<string> giveFeedBack)
		{
			var commandsInfo = "<color=green>name\t\t|\t\tdescription\n</color>";
			_console.Commands.ForEach(c => commandsInfo +=$"{c.Name}\t\t|\t\t{c.Description}\n");
			giveFeedBack?.Invoke(commandsInfo);
		}

		public string Name => "help";
		public IEnumerable<string> Aliases { get; }
		public string Description { get; }
	}
}