using System;
using System.Collections.Generic;
using Plugins.DebugSystem.Console.Commands;
using UnityEngine;

namespace Characters.Enemies
{
    [CreateAssetMenu(menuName = "Debug/Commands/Kill All Enemies", fileName = "KillAllCommand", order = 0)]
    public class KillAllCommand : CommandSO
    {
        public override void Execute(string[] args, Action<string> writeToConsole)
        {
            var enemies = FindObjectsOfType<Enemy>();
            foreach (var enemy in enemies)
            {
                var damage = enemy.Model.LifePoints;
                enemy.TakeDamage(damage);
                writeToConsole($"{enemy.name} took damage for {damage}HP");
            }
        }

        public override string Name => name;
        [SerializeField] private List<string> aliases;
        public override IEnumerable<string> Aliases => aliases;
        [SerializeField] private string _description = "Kills all enemies found";
        public override string Description => _description;
    }
}