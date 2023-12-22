using System.Collections.Generic;
using UnityEngine;

namespace Characters.Enemies.Planning
{
    [CreateAssetMenu(menuName = "Models/States/Effects/Damage Pawn", fileName = "DamagePawn", order = 0)]
    public class DamagePawn : Effect
    {
        [SerializeField] private int damageAmount = 1;
        
        public override void Apply(Dictionary<string, object> worldState, Pawn pawn)
        {
            pawn.Damageable.TakeDamage(damageAmount);
        }
    }
}