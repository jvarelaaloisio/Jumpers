using AI.GOAP;
using Core.Providers;
using UnityEngine;

namespace Characters.Enemies.Planning.StateReaders
{
    [CreateAssetMenu(menuName = "Models/States/World Reader/Read Key owner", fileName = "Reader_KeyOwner", order = 0)]
    public class ReadKeyOwner : WorldStateReader
    {
        [SerializeField] private string id = "KeyOwner";
        [SerializeField] private DataProvider<PickKeyAction> keyProvider;
        
        public override void PopulateState(GoapState state, Pawn pawn)
        {
            if (keyProvider == null)
            {
                Debug.LogWarning($"{nameof(keyProvider)} is null!");
                return;
            }
            if (keyProvider.Value == null)
            {
                Debug.LogWarning($"{nameof(keyProvider)}.{nameof(keyProvider.Value)} is null!");
                return;
            }
            if (!state.Strings.TryAdd(id, keyProvider.Value.ownerTag))
                state.Strings[id] = keyProvider.Value.ownerTag;
        }
    }
}