using System;
using UnityEngine;

namespace AI.GOAP.States.Conditions
{
    [CreateAssetMenu(menuName = "Models/States/Float", fileName = "FSP_", order = 0)]
    public class FloatConditionProvider : TypedConditionProvider<float>
    {
        [SerializeField] private NumberComparison comparison;

        protected override bool IsConditionMet_Internal(float myValue, float worldValue)
        {
            switch (comparison)
            {
                case NumberComparison.Greater:
                    return worldValue > myValue;
                case NumberComparison.GreaterOrEqual:
                    return worldValue >= myValue;
                case NumberComparison.Equal:
                    return Math.Abs(worldValue - myValue) < float.Epsilon;
                case NumberComparison.LessOrEqual:
                    return worldValue <= myValue;
                case NumberComparison.Less:
                    return worldValue < myValue;
                default:
                    Debug.LogError($"{name}: comparison type not found!");
                    return false;
            }
        }
    }
}