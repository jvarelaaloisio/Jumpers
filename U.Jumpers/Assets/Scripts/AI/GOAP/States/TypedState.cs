using System;
using Core.States;

namespace AI.GOAP
{
    [Serializable]
    public class TypedState<T> : State
    {
        public new T Value;
    }
}