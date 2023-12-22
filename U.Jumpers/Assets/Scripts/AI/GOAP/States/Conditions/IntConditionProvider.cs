using Core.States;
using UnityEngine;

namespace AI.GOAP.States.Conditions
{
    [CreateAssetMenu(menuName = "Models/States/Int", fileName = "ICP_", order = 0)]
    public class IntConditionProvider : TypedConditionProvider<int>
    {
        [SerializeField] private NumberComparison comparison;

        protected override bool IsConditionMet_Internal(int myValue, int worldValue)
        {
            switch (comparison)
            {
                case NumberComparison.Greater:
                    return worldValue > myValue;
                case NumberComparison.GreaterOrEqual:
                    return worldValue >= myValue;
                case NumberComparison.Equal:
                    return worldValue == myValue;
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