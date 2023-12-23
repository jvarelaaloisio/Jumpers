using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace Characters.Enemies.Planning.Actions
{
    public class DamageAction : GoapAction
    {
        [SerializeField] private int damageAmount = 1;
        [SerializeField] private float afterActionDelay = 1;
        [SerializeField] private UnityEvent onDamage;
        
        public override IEnumerator Do(Pawn pawn)
        {
            pawn.GetMonoBehaviour.GetComponent<CharacterView>().TakeDamage(damageAmount);
            onDamage.Invoke();
            yield return new WaitForSeconds(afterActionDelay);
            gameObject.SetActive(false);
        }
    }
}