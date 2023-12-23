using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace Characters.Enemies.Planning.Actions
{
    public class HealAction : GoapAction
    {
        [SerializeField] private int healAmount = 1;
        [SerializeField] private float afterActionDelay = 1;
        [SerializeField] private UnityEvent onHeal;
        
        public override IEnumerator Do(Pawn pawn)
        {
            pawn.GetMonoBehaviour.GetComponent<CharacterView>().TakeDamage(-healAmount);
            onHeal.Invoke();
            yield return new WaitForSeconds(afterActionDelay);
            gameObject.SetActive(false);
        }
    }
}