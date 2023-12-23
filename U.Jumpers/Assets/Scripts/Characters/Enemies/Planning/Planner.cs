using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using AI.GOAP;
using AI.GOAP.States.Conditions;
using UnityEngine;

namespace Characters.Enemies.Planning
{
    [Obsolete]
    [RequireComponent(typeof(Goap))]
    public class Planner : MonoBehaviour
    {
        [field: SerializeField] public GoapAction[] Actions { get; set; }
        [field: SerializeField] public ConditionProvider[] GoalConditions { get; set; }
        [field: SerializeField] public WorldStateReader[] WorldStateReaders { get; set; }
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
            StartPlanningGoap(currentState, goal, goap.TryPlan(currentState,
                                                               goal,
                                                               state => DoesSatisfy(state),
                                                               state => GetHeuristic(state),
                                                               Actions,
                                                               null));
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

        private void HandlePlan(IEnumerable<IGoapAction> obj)
        {
            throw new NotImplementedException();
        }

        public void StartPlanningGoap(GoapState currentState, GoapState goal, IEnumerator tryPlan)
        {
            if (_planningCor != null)
                StopCoroutine(_planningCor);
            _planningCor = StartCoroutine(tryPlan);
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