using AI.GOAP;
using UnityEngine;

namespace Characters.Enemies.Planning.Effects
{
    [CreateAssetMenu(menuName = "Models/States/Effects/Heal Pawn", fileName = "HealPawn", order = 0)]
    public class HealEffect : Effect
    {
        [SerializeField] private int healAmount = 1;
        [SerializeField] private string id = "selfHP";
        
        public override void Apply(GoapState state, Pawn pawn)
        {
            if (state.Ints.TryGetValue(id, out var value))
                state.Ints[id] = value + healAmount;
        }
    }
}