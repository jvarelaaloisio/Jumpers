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
        public abstract bool IsConditionMet(GoapState state);

        /// <summary>
        /// Used to populate the goal state
        /// </summary>
        /// <param name="state"></param>
        public abstract void ApplyConditionTo(GoapState state);

        public abstract int GetHeuristic(GoapState state);
    }
}