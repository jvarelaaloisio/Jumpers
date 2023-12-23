using System.Collections.Generic;
using UnityEngine;

namespace AI.GOAP.States.Conditions
{
    [CreateAssetMenu(menuName = "Models/States/Int", fileName = "ICP_", order = 0)]
    public class IntConditionProvider : TypedConditionProvider<int>
    {
        [SerializeField] private NumberComparison comparison;

        public override bool IsConditionMet(GoapState state)
        {
            if (!state.Ints.ContainsKey(conditionState.id))
                return false;
            var world = state.Ints[conditionState.id];
            var condition = conditionState.typedValue;
            switch (comparison)
            {
                case NumberComparison.Greater:
                    return world > condition;
                case NumberComparison.GreaterOrEqual:
                    return world >= condition;
                case NumberComparison.Equal:
                    return world == condition;
                case NumberComparison.LessOrEqual:
                    return world <= condition;
                case NumberComparison.Less:
                    return world < condition;
                default:
                    Debug.LogError($"{name}: comparison type not found!");
                    return false;
            }
        }

        public override void ApplyConditionTo(GoapState state)
        {
            if (!state.Ints.TryAdd(conditionState.id, conditionState.typedValue))
                state.Ints[conditionState.id] = conditionState.typedValue;
        }

        public override int GetHeuristic(GoapState state)
        {
            var world = state.Ints.GetValueOrDefault(conditionState.id, 0);
            var condition = conditionState.typedValue;
            switch (comparison)
            {
                case NumberComparison.Greater:
                    return Mathf.Clamp(condition + 1 - world, 0, int.MaxValue);
                case NumberComparison.GreaterOrEqual:
                    return Mathf.Clamp(condition - world, 0, int.MaxValue);
                case NumberComparison.Equal:
                    return Mathf.Abs(condition - world);
                case NumberComparison.LessOrEqual:
                    return Mathf.Clamp(world - condition, 0, int.MaxValue);
                case NumberComparison.Less:
                    return Mathf.Clamp(world + 1 - condition, 0, int.MaxValue);
                default:
                    Debug.LogError($"{name}: comparison type not found!");
                    return int.MaxValue;
            }
        }
    }
}