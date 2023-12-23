using System.Collections;
using System.Linq;
using AI.GOAP;
using AI.GOAP.States.Conditions;
using Core.Providers;
using UnityEngine;

namespace Characters.Enemies.Planning
{
    public abstract class GoapAction : MonoBehaviour, IGoapAction
    {
        [field: SerializeField] public string Name { get; protected set; }
        [field: SerializeField] public float Cost { get; protected set; } = 1;

        [SerializeField] private ListDataProvider<GoapAction> actionsProvider;
        [SerializeField] protected ConditionProvider[] preconditions;
        [SerializeField] protected Effect[] effects;
        public Vector3 Position => transform.position;

        private void OnValidate()
        {
            if (string.IsNullOrEmpty(Name) || string.IsNullOrWhiteSpace(Name))
                Name = name;
        }

        protected virtual void OnEnable()
        {
            if (!actionsProvider)
            {
                Debug.LogError($"{nameof(actionsProvider)} is null!", this);
                return;
            }
            actionsProvider.Value.Add(this);
        }

        protected virtual void OnDisable()
        {
            if (actionsProvider)
                actionsProvider.Value.Remove(this);
        }

        /// <summary>
        /// Does the given state meet the preconditions set for this action?
        /// </summary>
        /// <param name="state"></param>
        /// <returns></returns>
        public bool DoesMeetPreconditions(GoapState state)
            => preconditions.All(pre => pre.IsConditionMet(state));

        /// <summary>
        /// Modify state based on action consequences
        /// </summary>
        /// <param name="state"></param>
        /// <param name="pawn"></param>
        /// <returns></returns>
        public GoapState ApplyEffects(GoapState state, Pawn pawn)
        {
            var afterEffects = new GoapState(state);
            foreach (var provider in effects) provider.Apply(afterEffects, pawn);
            return afterEffects;
        }

        public abstract IEnumerator Do(Pawn pawn);
    }
}