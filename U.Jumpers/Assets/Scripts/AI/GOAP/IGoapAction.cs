using System.Collections;
using Characters;
using UnityEngine;

namespace AI.GOAP
{
    public interface IGoapAction
    {
        string Name { get; }
        float Cost { get; }
        Vector3 Position { get; }
        bool DoesMeetPreconditions(GoapState state);
        GoapState ApplyEffects(GoapState state, Pawn pawn);
        IEnumerator Do(Pawn pawn);
    }
}