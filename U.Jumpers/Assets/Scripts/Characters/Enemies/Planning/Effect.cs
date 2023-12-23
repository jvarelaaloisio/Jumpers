using System.Collections.Generic;
using AI.GOAP;
using UnityEngine;

namespace Characters.Enemies.Planning
{
    public abstract class Effect : ScriptableObject
    {
        public abstract void Apply(GoapState state, Pawn pawn);
    }
}