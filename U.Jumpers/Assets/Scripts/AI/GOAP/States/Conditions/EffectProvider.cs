using System.Collections.Generic;
using UnityEngine;

namespace AI.GOAP.States.Conditions
{
    public abstract class EffectProvider : ScriptableObject
    {
        public abstract void ApplyEffect(Dictionary<string, object> worldState);
    }
}