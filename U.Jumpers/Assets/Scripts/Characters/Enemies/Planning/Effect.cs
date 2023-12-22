using System.Collections.Generic;
using UnityEngine;

namespace Characters.Enemies.Planning
{
    public abstract class Effect : ScriptableObject
    {
        public abstract void Apply(Dictionary<string, object> worldState, Pawn pawn);
    }
}