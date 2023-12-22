using System.Collections.Generic;
using UnityEngine;

namespace Characters.Enemies.Planning
{
    [CreateAssetMenu(menuName = "Models/States/Effects/Heal Pawn", fileName = "HealPawn", order = 0)]
    public class HealPawn : Effect
    {
        [SerializeField] private int healAmount = 1;
        
        public override void Apply(Dictionary<string, object> worldState, Pawn pawn)
        {
            pawn.Damageable.TakeDamage(-healAmount);
        }
    }
}