using System.Collections.Generic;
using UnityEngine;

namespace Characters.Enemies.Planning
{
    [CreateAssetMenu(menuName = "Models/States/World Reader/Read Pawn Health", fileName = "Reader_PawnHealth", order = 0)]
    public class ReadPawnHealth : WorldStateReader
    {
        [SerializeField] private string id = "selfHP";
        
        public override void PopulateState(ref Dictionary<string, object> worldState, Pawn pawn)
        {
            var hp = pawn.Damageable.LifePoints;
            if (!worldState.TryAdd(id, hp))
            {
                worldState[id] = hp;
            }
        }
    }
}