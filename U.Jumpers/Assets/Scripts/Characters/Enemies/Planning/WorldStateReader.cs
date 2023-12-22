using System.Collections.Generic;
using UnityEngine;

namespace Characters.Enemies.Planning
{
    public abstract class WorldStateReader : ScriptableObject
    {
        /// <summary>
        /// Assigns the values correspondent to this world reader 
        /// </summary>
        /// <param name="worldState"></param>
        /// <param name="pawn"></param>
        public abstract void PopulateState(ref Dictionary<string, object> worldState, Pawn pawn);
    }
}