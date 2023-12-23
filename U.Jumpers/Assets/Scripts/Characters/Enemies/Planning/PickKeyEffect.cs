using AI.GOAP;
using UnityEngine;

namespace Characters.Enemies.Planning
{
    [CreateAssetMenu(menuName = "Models/States/Effects/Pick Key", fileName = "PickKey", order = 0)]
    public class PickKeyEffect : Effect
    {
        [SerializeField] private string id = "KeyOwner";
        
        public override void Apply(GoapState state, Pawn pawn)
        {
            var value = pawn.GetTransform.tag;
            if (!state.Strings.TryAdd(id, value))
                state.Strings[id] = value;
        }
    }
}