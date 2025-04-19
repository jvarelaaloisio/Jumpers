using System;
using System.Collections.Generic;
using Plugins.DebugSystem.Console.Commands;
using UnityEngine;

namespace Characters.Enemies
{
    [CreateAssetMenu(menuName = "Debug/Commands/Kill All Enemies", fileName = "KillAllCommand", order = 0)]
    public class KillAllCommand : CommandSo
    {
        public override void Execute(Action<string> writeToConsole, params string[] args)
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
        [SerializeField] private string description = "Kills all enemies found";
        public override string Description => description;
    }
}