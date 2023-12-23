using System;
using Core.States;
using UnityEngine;

namespace AI.GOAP.States
{
    [Serializable]
    public class TypedState<T> : State
    {
        [SerializeField]
        public T typedValue;

        public override object GetValue() => typedValue;
    }
}