using System;
using AI.GOAP;
using UnityEngine;

namespace Characters.Enemies.Planning.StateReaders
{
    [CreateAssetMenu(menuName = "Models/States/World Reader/Read Pawn Health", fileName = "Reader_PawnHealth", order = 0)]
    public class ReadPawnHealth : WorldStateReader
    {
        [SerializeField] private string id = "selfHP";
        
        public override void PopulateState(GoapState state, Pawn pawn)
        {
            var hp = pawn.Damageable.LifePoints;
            if (!state.Ints.TryAdd(id, hp))
            {
                state.Ints[id] = hp;
            }
        }
    }
}