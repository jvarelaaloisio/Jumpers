using System.Collections.Generic;
using Core.States;
using UnityEngine;

namespace AI.GOAP.States.Conditions
{
    public abstract class ConditionProvider : ScriptableObject
    {
        protected abstract State GetConditionState();
        
        /// <summary>
        /// Is this state value the same as the one found in the given world state
        /// </summary>
        /// <param name="worldState"></param>
        /// <returns></returns>
        public virtual bool IsConditionMet(Dictionary<string, object> worldState)
        {
            if (worldState.TryGetValue(GetConditionState().id, out var sateValue))
                return sateValue == GetConditionState().Value;
            return false;
        }
    }
}