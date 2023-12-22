using System.Collections.Generic;
using System.Linq;
using Core.States;
using UnityEngine;

namespace AI.GOAP.States.Conditions
{
    public abstract class TypedConditionProvider<T> : ConditionProvider
    {
        [SerializeField] private TypedState<T> conditionState;

        protected override State GetConditionState()
        {
            return conditionState;
        }

        public override bool IsConditionMet(Dictionary<string, object> worldState)
        {
            var typedState = (TypedState<T>)GetConditionState();
            if (!worldState.TryGetValue(typedState.id, out var currentValue))
            {
                currentValue = 0;
                worldState.TryAdd(typedState.id, currentValue);
            }

            var typedWorldState = (T)currentValue;
            return IsConditionMet_Internal(typedState.value, typedWorldState);
        }

        protected virtual bool IsConditionMet_Internal(T myValue, T worldValue)
        {
            return myValue.Equals(worldValue);
        }
    }
}