using System;
using System.Collections.Generic;
using DS.DebugConsole;
using UnityEngine;

namespace Plugins.DebugSystem.Console.Commands
{
    public abstract class CommandSO : ScriptableObject, ICommand<string>
    {
        public abstract void Execute(string[] args, Action<string> giveFeedBack);

        public abstract string Name { get; }
        public abstract IEnumerable<string> Aliases { get; }
        public abstract string Description { get; }
    }
}
