using System.Collections.Generic;
using AI.GOAP.States.Conditions;
using UnityEngine;

namespace AI.GOAP.States
{
    [CreateAssetMenu(menuName = "Models/States/String", fileName = "SSP_", order = 0)]
    public class StringConditionProvider : TypedConditionProvider<string> { }
}