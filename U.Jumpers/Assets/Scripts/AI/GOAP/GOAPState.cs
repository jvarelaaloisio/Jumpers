using System;
using System.Collections.Generic;
using System.Linq;

namespace AI.GOAP
{
    public class GoapState : IEquatable<GoapState>
    {
        /// <summary>
        /// World flags corresponding to this state
        /// </summary>
        public readonly Dictionary<string, object> WorldState;

        /// <summary>
        /// The action that generated this state
        /// </summary>
        public GoapAction GeneratingAction = null;

        public GoapState(GoapAction gen = null)
        {
            GeneratingAction = gen;
            WorldState = new Dictionary<string, object>();
        }

        public GoapState(GoapState source, GoapAction gen = null)
        {
            WorldState = new Dictionary<string, object>(source.WorldState);
            GeneratingAction = gen;
        }

        public bool Equals(GoapState other)
        {
            return other != null
                   && other.GeneratingAction == GeneratingAction
                   && other.WorldState.Values.Count == WorldState.Values.Count
                   && other.WorldState.Values.All(kv => WorldState.Values.Contains(kv));
        }

        public override bool Equals(object obj)
        {
            return obj is GoapState other
                   && Equals(other);
        }
        
        public override int GetHashCode()
        {
            return WorldState.Values.Count == 0
                       ? 0
                       : 31 * WorldState.Values.Count + 31 * 31 * WorldState.Values.First().GetHashCode();
        }

        public override string ToString()
        {
            var str = WorldState.Aggregate("",
                                           (current, state)
                                               => current + $"{state.Key} : {state.Value}\n");

            return ("--->" + (GeneratingAction != null ? GeneratingAction.Name : "NULL") + "\n" + str);
        }
    }
}