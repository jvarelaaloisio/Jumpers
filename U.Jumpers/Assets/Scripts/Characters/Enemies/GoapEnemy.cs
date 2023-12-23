using System.Collections;
using System.Collections.Generic;
using System.Linq;
using AI.GOAP;
using AI.GOAP.States.Conditions;
using Characters.Enemies.Planning;
using Core.Extensions;
using Core.Providers;
using Debugging;
using UnityEngine;

namespace Characters.Enemies
{
    [RequireComponent(typeof(Goap))]
    public class GoapEnemy : Enemy
    {
        [field: SerializeField] public ConditionProvider[] GoalConditions { get; set; }
        [field: SerializeField] public WorldStateReader[] WorldStateReaders { get; set; }

        [SerializeField] private ListDataProvider<GoapAction> actionsProvider;
        [SerializeField] private Goap goap;
        private Coroutine _planningCor;

        private void Reset()
        {
            goap = GetComponent<Goap>();
        }

        protected override void OnEnable()
        {
            if (actionsProvider == null)
            {
                Debug.LogError($"{name}: {nameof(actionsProvider)} is null!");
            }
            goap.OnPlanFound += HandlePlan;
            goap.OnPlanningFailed += HandlePlanningFailed;
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            goap.OnPlanFound -= HandlePlan;
            goap.OnPlanningFailed -= HandlePlanningFailed;
        }

        protected override void PlanNextMovement()
        {
            var currentState = GetCurrentState(WorldStateReaders, pawn);
            var goal = GetGoalState(GoalConditions);
            StartPlanningGoap(goap.TryPlan(currentState,
                                           goal,
                                           DoesSatisfy,
                                           GetHeuristic,
                                           actionsProvider.Value,
                                           pawn));
        }

        /// <summary>
        /// Handles the goap coroutine
        /// </summary>
        /// <param name="tryPlan"></param>
        private void StartPlanningGoap(IEnumerator tryPlan)
        {
            if (_planningCor != null)
                StopCoroutine(_planningCor);
            _planningCor = StartCoroutine(tryPlan);
        }

        /// <summary>
        /// Get the cost of an state compared to goal conditions
        /// </summary>
        /// <param name="state"></param>
        /// <returns></returns>
        private float GetHeuristic(GoapState state)
        {
            return GoalConditions.Sum(condition => condition.GetHeuristic(state));
        }

        /// <summary>
        /// Did we achieve the goal?
        /// </summary>
        /// <param name="state"></param>
        /// <returns></returns>
        private bool DoesSatisfy(GoapState state)
            => GoalConditions.All(condition => condition.IsConditionMet(state));

        /// <summary>
        /// All hope is lost, planning failed
        /// </summary>
        private void HandlePlanningFailed()
        {
            $"Planning Failed! :(".Log();
            PlanNextMovement();
        }

        /// <summary>
        /// Handles a given plan. Runs a coroutine.
        /// </summary>
        /// <param name="planSequence"></param>
        private void HandlePlan(IEnumerable<IGoapAction> planSequence)
        {
            StartCoroutine(DoPlanCoroutine(planSequence));
        }

        /// <summary>
        /// Does every action in a plan
        /// </summary>
        /// <param name="planSequence"></param>
        /// <returns></returns>
        private IEnumerator DoPlanCoroutine(IEnumerable<IGoapAction> planSequence)
        {
            Printer.Log($"{name}: Doing plan...");
            foreach (var goapAction in planSequence)
            {
                yield return MovePawnTo(goapAction);
                yield return goapAction.Do(pawn);
                Printer.Log($"{name}: did {goapAction.Name}");
            }
            
            Printer.Log($"{name}: finished plan");
        }

        /// <summary>
        /// Moves the pawn to a given action's position
        /// </summary>
        /// <param name="goapAction"></param>
        /// <returns></returns>
        private IEnumerator MovePawnTo(IGoapAction goapAction)
        {
            var isMoving = true;
            pawn.OnFinishedMoving += FinishedMoving;
            var actionPosition = goapAction.Position.XZ();
            while (Vector2.Distance(pawn.GetTransform.position.XZ(), actionPosition) > 0.001f)
            {
                var availablePillars = pawn.GetAvailablePillars(pawn.GetTransform.position);
                var pillarInDirectionOfGoapAction = availablePillars
                                                    .OrderBy(pilar => Vector2.Distance(pilar.position.XZ(),
                                                                                       actionPosition))
                                                    .FirstOrDefault();
                if (pillarInDirectionOfGoapAction == null)
                {
                    Printer.LogError($"{nameof(pillarInDirectionOfGoapAction)} is null!");
                    yield break;
                }
                pawn.MoveCharacter(pillarInDirectionOfGoapAction.position);
                isMoving = true;
                yield return new WaitWhile(() => isMoving);
            }

            pawn.OnFinishedMoving -= FinishedMoving;

            void FinishedMoving() => isMoving = false;
        }

        /// <summary>
        /// Reads the state and returns it
        /// </summary>
        /// <param name="worldStateReaders"></param>
        /// <param name="pawn"></param>
        /// <returns></returns>
        private static GoapState GetCurrentState(IEnumerable<WorldStateReader> worldStateReaders, Pawn pawn)
        {
            var current = new GoapState();
            foreach (var reader in worldStateReaders)
                reader.PopulateState(current, pawn);
            return current;
        }

        /// <summary>
        /// Applies all conditions to a state and returns it
        /// </summary>
        /// <param name="goalConditions"></param>
        /// <returns></returns>
        private static GoapState GetGoalState(IEnumerable<ConditionProvider> goalConditions)
        {
            var goal = new GoapState();
            foreach (var condition in goalConditions)
                condition.ApplyConditionTo(goal);

            return goal;
        }
    }
}