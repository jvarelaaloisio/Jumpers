using System.Collections.Generic;
using Core.Providers;
using UnityEngine;

namespace Characters.Enemies.Planning
{
    [CreateAssetMenu(menuName = "Models/Providers/Goap Actions", fileName = "ADP_")]
    public class GoapActionsDataProvider : ListDataProvider<GoapAction>
    {
        [ContextMenu("Log value")]
        private void LogValue()
            => Debug.Log($"{name}: {GetLogData()}");
    }
}
