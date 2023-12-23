using AI.GOAP;
using UnityEngine;

namespace Characters.Enemies.Planning
{
    [CreateAssetMenu(menuName = "Models/States/Effects/Eat Pizza", fileName = "EatPizza", order = 0)]
    public class EatPizzaEffect : Effect
    {
        [SerializeField] private string id = "Amount of pizza radians eaten";
        
        public override void Apply(GoapState state, Pawn pawn)
        {
            if (!state.Floats.TryAdd(id, 3.14f))
                state.Floats[id] = 3.14f;
        }
    }
}