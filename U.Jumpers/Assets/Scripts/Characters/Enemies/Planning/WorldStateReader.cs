using System.Collections.Generic;
using AI.GOAP;
using UnityEngine;

namespace Characters.Enemies.Planning
{
    public abstract class WorldStateReader : ScriptableObject
    {
        /// <summary>
        /// Assigns the values correspondent to this world reader 
        /// </summary>
        /// <param name="state"></param>
        /// <param name="pawn"></param>
        public abstract void PopulateState(GoapState state, Pawn pawn);
    }
}