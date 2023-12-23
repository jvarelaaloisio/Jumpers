using System.Collections.Generic;
using Core.States;
using UnityEngine;

namespace AI.GOAP.States.Conditions
{
    public abstract class TypedConditionProvider<T> : ConditionProvider
    {
        [SerializeField] protected TypedState<T> conditionState;

        protected override State GetConditionState()
            => conditionState;
    }
}