using System.Collections.Generic;
using UnityEngine;

namespace AI.GOAP.States.Conditions
{
    [CreateAssetMenu(menuName = "Models/States/Bool", fileName = "BSP_", order = 0)]
    public class BoolConditionProvider : TypedConditionProvider<bool>
    {
        public override void ApplyConditionTo(GoapState state)
        {
            var condition = (TypedState<bool>)GetConditionState();
            if (state.Bools.ContainsKey(condition.id))
                state.Bools[condition.id] = condition.typedValue;
        }

        public override int GetHeuristic(GoapState state)
        {
            return IsConditionMet(state) ? 0 : 1;
        }

        public override bool IsConditionMet(GoapState state)
        {
            var condition = (TypedState<bool>)GetConditionState();
            if (state.Bools.TryGetValue(condition.id, out var worldValue))
                return worldValue == condition.typedValue;
            return false;
        }
    }
}