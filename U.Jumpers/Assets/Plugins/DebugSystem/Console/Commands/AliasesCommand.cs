using System;
using System.Collections.Generic;
using System.Linq;
using DS.DebugConsole;
using Plugins.DebugSystem.Console.Extensions;
using UnityEngine;

namespace Plugins.DebugSystem.Console.Commands
{
    public class AliasesCommand : ICommand<string>
    {
        private readonly IDebugConsole<string> _console;

        public AliasesCommand(IDebugConsole<string> console) => _console = console;
        public void Execute(Action<string> log, params string[] args)
        {
            var cmdNameOrAlias = args[0];
			
            if (_console.IsValidCommand(cmdNameOrAlias))
            {
                var command = _console.Commands.FirstOrDefault(c => c.Name == cmdNameOrAlias || c.Aliases.Contains(cmdNameOrAlias));
                if (command != null)
                    log($"{command.Name} => [{command.Aliases.Aggregate("", (current, alias) => $"{current}, {alias}")}]");
                return;
            }

            log($"Command not found: {cmdNameOrAlias.Colored(Color.red)}");
        }

        public string Name => "alias";
        public IEnumerable<string> Aliases => new [] { "aliases", "ALIAS", "ALIASES" };
        public string Description => "Logs the aliases for the given command";
    }
}