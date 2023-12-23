using Characters.Enemies.Planning.StateReaders;
using Core.Providers;
using UnityEngine;

namespace Characters.Enemies.Planning
{
    [CreateAssetMenu(menuName = "Models/Providers/Key", fileName = "KDP_", order = 0)]
    public class KeyDataProvider : DataProvider<PickKeyAction>
    {
        public override PickKeyAction Value { get; set; }
    }
}