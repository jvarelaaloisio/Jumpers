using System.Linq;
using AI.GOAP;
using AI.GOAP.States.Conditions;
using UnityEngine;

namespace Characters.Enemies.Planning
{
    [CreateAssetMenu(menuName = "Models/AI/GOAP/Action Container", fileName = "GAC_")]
    public class GoapActionContainer : ScriptableObject
    {
        [SerializeField] protected ConditionProvider[] preconditions;
        [SerializeField] protected Effect[] effects;
        [SerializeField] private new string name;
        [SerializeField] protected float cost = 1;

        private void OnValidate()
        {
            if (string.IsNullOrEmpty(name) || string.IsNullOrWhiteSpace(name))
                name = base.name;
        }

        public virtual GoapAction GetAction(Pawn pawn)
        {
            return new GoapAction(name,
                                  state => preconditions.All(pre => pre.IsConditionMet(state.WorldState)),
                                  state =>
                                  {
                                      var afterEffects = new GoapState(state);
                                      foreach (var provider in effects) provider.Apply(afterEffects.WorldState, pawn);
                                      return afterEffects;
                                  },
                                  cost);
        }
    }
}