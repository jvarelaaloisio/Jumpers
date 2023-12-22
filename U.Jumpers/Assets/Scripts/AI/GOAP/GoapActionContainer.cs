using System.Linq;
using AI.GOAP.States;
using AI.GOAP.States.Conditions;
using UnityEngine;

namespace AI.GOAP
{
    [CreateAssetMenu(menuName = "Models/AI/GOAP/Action Container", fileName = "GAC_")]
    public class GoapActionContainer : ScriptableObject
    {
        [SerializeField] protected ConditionProvider[] preconditions;
        [SerializeField] protected EffectProvider[] effects;
        [SerializeField] private new string name;
        [SerializeField] protected float cost = 1;

        private void OnValidate()
        {
            if (string.IsNullOrEmpty(name) || string.IsNullOrWhiteSpace(name))
                name = base.name;
        }

        public virtual GoapAction GetAction()
        {
            return new GoapAction(name,
                                  state => preconditions.All(pre => pre.IsConditionMet(state.WorldState)),
                                  state =>
                                  {
                                      var afterEffects = new GoapState(state);
                                      foreach (var provider in effects) provider.ApplyEffect(afterEffects.WorldState);
                                      return afterEffects;
                                  },
                                  cost);
        }
    }
}