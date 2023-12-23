using System.Collections;
using Core.Providers;
using UnityEngine;
using UnityEngine.Events;

namespace Characters.Enemies.Planning.StateReaders
{
    public class OpenDoorAction : GoapAction
    {
        [SerializeField] private float afterActionDelay = 1;
        [SerializeField] private UnityEvent onOpen;
        [SerializeField] private DataProvider<PickKeyAction> keyProvider;
        
        public override IEnumerator Do(Pawn pawn)
        {
            if (keyProvider && keyProvider.Value)
            {
                keyProvider.Value.gameObject.SetActive(false);
            }
            onOpen.Invoke();
            yield return new WaitForSeconds(afterActionDelay);
        }
    }
}