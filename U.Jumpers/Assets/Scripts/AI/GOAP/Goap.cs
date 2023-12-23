using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using AI.AStar;
using Characters;
using Core.Helpers;
using UnityEngine;

namespace AI.GOAP
{
    using Arc = AStar<GoapState>.Arc;
    using PathResult = AStar<GoapState>.PathResult;

    public class Goap : MonoBehaviour
    {
        [SerializeField] private int maxIterationsPerFrame = 50;
        [SerializeField] private Period timeSlicingPeriod = new();

        public event Action<IEnumerable<IGoapAction>> OnPlanFound = delegate { };
        public event Action OnPlanningFailed = delegate { };

        private readonly AStar<GoapState> _aStar = new();

        public IEnumerator TryPlan(GoapState current,
                                   GoapState goal,
                                   Func<GoapState, bool> doesSatisfy,
                                   Func<GoapState, float> getHeuristic,
                                   IEnumerable<IGoapAction> actions,
                                   Pawn pawn)
        {
            var result = new PathResult(goal, null, false);

            foreach (var path in FindPossiblePath())
            {
                Debug.Log($"{name}: Partial path is " +
                          $"{path.Steps.Value.Aggregate("", (s, state) => $"\n{state.GeneratingAction?.Name ?? "NULL"} -> [{LogState(state.Bools)}], [{LogState(state.Ints)}], [{LogState(state.Floats)}], [{LogState(state.Strings)}]")}");
                if (path.IsValid)
                {
                    result = path;
                    break;
                }

                yield return timeSlicingPeriod.Wait();
            }

            if (!result.IsValid)
            {
                OnPlanningFailed();
                yield break;
            }

            OnPlanFound(result.Steps.Value.Skip(1).Select(x => x.GeneratingAction));
            yield break;

            IEnumerable<PathResult> FindPossiblePath() =>
                _aStar.FindPath(current,
                                goal,
                                (curr, _) => getHeuristic(curr),
                                doesSatisfy,
                                state => actions
                                         .Where(a => a.DoesMeetPreconditions(state))
                                         .Select(action =>
                                         {
                                             var newState = new GoapState(state);
                                             newState = action.ApplyEffects(newState, pawn);
                                             newState.GeneratingAction = action;
                                             return new Arc(newState, action.Cost);
                                         }),
                                maxIterationsPerFrame);

            string LogState<T>(Dictionary<string, T> values)
            {
                return values?.Aggregate("", (total, pair) => $"{total}, ({pair.Key}, {pair.Value})");
            }
        }
    }
}