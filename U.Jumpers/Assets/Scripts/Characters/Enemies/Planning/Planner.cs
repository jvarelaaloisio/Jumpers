using System;
using System.Collections.Generic;
using System.Linq;
using AI.GOAP;
using UnityEngine;

namespace Characters.Enemies.Planning
{
    [RequireComponent(typeof(Goap))]
    public class Planner : MonoBehaviour
    {
        [SerializeField] private GoapActionContainer[] actions;
        [SerializeField] private WorldStateReader[] worldStateReaders;
        [SerializeField] private CharacterView characterView;
        [SerializeField] private Goap goap;
        private Coroutine _planningCor;

        private void Reset()
        {
            goap = GetComponent<Goap>();
        }

        private void OnEnable()
        {
            if (!goap)
            {
                Debug.LogError($"{nameof(goap)} is null!");
                enabled = false;
                return;
            }

            goap.OnPlanFound += HandlePlan;
            goap.OnPlanningFailed += HandlePlanningFailed;
            //getCurrentWorldState
            var currentState = new GoapState();
            //get actions
            //get goal world state
            var goal = new GoapState();
            ExecuteGoap(currentState, goal);
            //if plan != null
            //yield return pawn.doAction(plan.NextAction)
            
        }

        private void OnDisable()
        {
            goap.OnPlanFound -= HandlePlan;
            goap.OnPlanningFailed -= HandlePlanningFailed;
        }

        private void HandlePlanningFailed()
        {
            throw new NotImplementedException();
        }

        private void HandlePlan(IEnumerable<GoapAction> obj)
        {
            throw new NotImplementedException();
        }

        private void ExecuteGoap(GoapState currentState, GoapState goal)
        {
            if (_planningCor != null)
                StopCoroutine(_planningCor);
            _planningCor = StartCoroutine(goap.TryPlan(currentState,
                                                        goal,
                                                        state => DoesSatisfy(state),
                                                        state => GetHeuristic(state),
                                                        actions.Select(action => action.GetAction(characterView.Pawn))));
        }

        private float GetHeuristic(GoapState state)
        {
            throw new NotImplementedException();
        }

        private bool DoesSatisfy(GoapState state)
        {
            throw new NotImplementedException();
        }
    }
}