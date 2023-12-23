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
        public readonly Dictionary<string, bool> Bools = new();
        public readonly Dictionary<string, int> Ints = new();
        public readonly Dictionary<string, float> Floats = new();
        public readonly Dictionary<string, string> Strings = new();

        /// <summary>
        /// The action that generated this state
        /// </summary>
        public IGoapAction GeneratingAction = null;

        public GoapState(IGoapAction gen = null)
        {
            GeneratingAction = gen;
        }

        public GoapState(GoapState source, IGoapAction gen = null)
        {
            Bools = new Dictionary<string, bool>(source.Bools);
            Ints = new Dictionary<string, int>(source.Ints);
            Floats = new Dictionary<string, float>(source.Floats);
            Strings = new Dictionary<string, string>(source.Strings);
            GeneratingAction = gen;
        }

        public bool Equals(GoapState other)
        {
            return other != null
                   && other.GeneratingAction == GeneratingAction
                   && other.Strings.Values.Count == Strings.Values.Count
                   && other.Strings.Values.All(kv => Strings.Values.Contains(kv))
                   && other.Bools.Values.Count == Bools.Values.Count
                   && other.Bools.Values.All(kv => Bools.Values.Contains(kv))
                   && other.Ints.Values.Count == Ints.Values.Count
                   && other.Ints.Values.All(kv => Ints.Values.Contains(kv))
                   && other.Floats.Values.Count == Floats.Values.Count
                   && other.Floats.Values.All(kv => Floats.Values.Contains(kv));
        }

        public override bool Equals(object obj)
        {
            return obj is GoapState other
                   && Equals(other);
        }
        
        public override int GetHashCode()
        {
            return Strings.Values.Count == 0
                       ? 0
                       : 31 * Strings.Values.Count + 31 * 31 * Strings.Values.First().GetHashCode();
        }

        public override string ToString()
        {
            var str = Strings.Aggregate("",
                                           (current, state)
                                               => current + $"{state.Key} : {state.Value}\n");

            return ("--->" + (GeneratingAction != null ? GeneratingAction.Name : "NULL") + "\n" + str);
        }
    }
}