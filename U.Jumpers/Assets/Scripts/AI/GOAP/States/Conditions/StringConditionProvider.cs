using System.Collections.Generic;
using UnityEngine;

namespace AI.GOAP.States.Conditions
{
    [CreateAssetMenu(menuName = "Models/States/String", fileName = "SSP_", order = 0)]
    public class StringConditionProvider : TypedConditionProvider<string>
    {
        [Tooltip("The heuristic to return when condition is not met")]
        [SerializeField] private int heuristic;

        public override bool IsConditionMet(GoapState state)
        {
            var condition = conditionState.typedValue;
            var worldValue = state.Strings.GetValueOrDefault(conditionState.id, "NULL VALUE");
            return condition == worldValue;
        }

        public override void ApplyConditionTo(GoapState state)
        {
            if (!state.Strings.TryAdd(conditionState.id, conditionState.typedValue))
                state.Strings[conditionState.id] = conditionState.typedValue;
        }

        public override int GetHeuristic(GoapState state) => IsConditionMet(state) ? 0 : heuristic;
    }
}