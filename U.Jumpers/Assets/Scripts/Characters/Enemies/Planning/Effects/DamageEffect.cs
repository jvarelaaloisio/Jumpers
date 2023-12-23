using System.Collections.Generic;
using AI.GOAP;
using UnityEngine;

namespace Characters.Enemies.Planning.Effects
{
    [CreateAssetMenu(menuName = "Models/States/Effects/Damage Pawn", fileName = "DamagePawn", order = 0)]
    public class DamageEffect : Effect
    {
        [SerializeField] private int damageAmount = 1;
        [SerializeField] private string id = "selfHP";
        
        public override void Apply(GoapState state, Pawn pawn)
        {
            if (state.Ints.TryGetValue(id, out var value))
                state.Ints[id] = value - damageAmount;
        }
    }
}