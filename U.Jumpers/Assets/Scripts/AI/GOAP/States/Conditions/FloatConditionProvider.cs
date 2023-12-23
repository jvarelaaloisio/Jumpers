using System;
using System.Collections.Generic;
using UnityEngine;

namespace AI.GOAP.States.Conditions
{
    [CreateAssetMenu(menuName = "Models/States/Float", fileName = "FSP_", order = 0)]
    public class FloatConditionProvider : TypedConditionProvider<float>
    {
        [SerializeField] private NumberComparison comparison;

        public override bool IsConditionMet(GoapState state)
        {
            var world = state.Floats.GetValueOrDefault(conditionState.id, 0);
            var condition = conditionState.typedValue;
            switch (comparison)
            {
                case NumberComparison.Greater:
                    return world > condition;
                case NumberComparison.GreaterOrEqual:
                    return world >= condition;
                case NumberComparison.Equal:
                    return Math.Abs(world - condition) < float.Epsilon;
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
            if (!state.Floats.TryAdd(conditionState.id, conditionState.typedValue))
                state.Floats[conditionState.id] = conditionState.typedValue;
        }

        public override int GetHeuristic(GoapState state)
        {
            var world = state.Floats.GetValueOrDefault(conditionState.id, 0);
            var condition = conditionState.typedValue;
            switch (comparison)
            {
                case NumberComparison.Greater:
                    return (int)Mathf.Clamp(condition + 1 - world, 0, float.MaxValue);
                case NumberComparison.GreaterOrEqual:
                    return (int)Mathf.Clamp(condition - world, 0, float.MaxValue);
                case NumberComparison.Equal:
                    return (int)Mathf.Abs(condition - world);
                case NumberComparison.LessOrEqual:
                    return (int)Mathf.Clamp(world - condition, 0, float.MaxValue);
                case NumberComparison.Less:
                    return (int)Mathf.Clamp(world + 1 - condition, 0, float.MaxValue);
                default:
                    Debug.LogError($"{name}: comparison type not found!");
                    return int.MaxValue;
            }
        }
    }
}