using System;
using System.Collections.Generic;
using UnityEngine;

namespace AI.GOAP
{
    /// <summary>
    /// Esta parte transformado en utilizar Funcs, pero por ahora hay una mezcla
    /// </summary>
    public class GoapAction
    {
        /// <summary>
        /// Does the given state meet the preconditions set for this action?
        /// </summary>
        public Func<GoapState, bool> DoesMeetPreconditions;

        /// <summary>
        /// Modify state based on action consequences
        /// </summary>
        public Func<GoapState, GoapState> ApplyEffects;

        public float Cost { get; private set; }
        public string Name { get; private set; }

        public GoapAction(string name,
                          Func<GoapState, bool> doesMeetPreconditions,
                          Func<GoapState, GoapState> applyEffects,
                          float cost = 1)
        {
            Name = name;
            ApplyEffects = applyEffects;
            DoesMeetPreconditions = doesMeetPreconditions;
            Cost = cost;
        }
    }
}
