using System.Collections;
using Core.Providers;
using UnityEngine;
using UnityEngine.Events;

namespace Characters.Enemies.Planning.StateReaders
{
    public class PickKeyAction : GoapAction
    {
        [SerializeField] private DataProvider<PickKeyAction> keyProvider;
        [SerializeField] private float afterActionDelay = 1;
        [SerializeField] private UnityEvent onPick;
        
        public string ownerTag = string.Empty;
        protected override void OnEnable()
        {
            base.OnEnable();
            keyProvider.Value = this;
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            keyProvider.Value = null;
        }

        public override IEnumerator Do(Pawn pawn)
        {
            ownerTag = pawn.GetTransform.tag;
            transform.SetParent(pawn.GetTransform);
            onPick.Invoke();
            yield return new WaitForSeconds(afterActionDelay);
        }
    }
}