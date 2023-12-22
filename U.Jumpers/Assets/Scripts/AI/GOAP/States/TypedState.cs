using System;
using Core.States;
using UnityEngine;

namespace AI.GOAP
{
    [Serializable]
    public class TypedState<T> : State
    {
        [SerializeField]
        public new T value;
    }
}