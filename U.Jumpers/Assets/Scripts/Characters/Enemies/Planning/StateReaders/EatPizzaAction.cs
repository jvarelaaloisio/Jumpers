using System.Collections;
using Debugging;
using UnityEngine;
using UnityEngine.Events;

namespace Characters.Enemies.Planning.StateReaders
{
    public class EatPizzaAction : GoapAction
    {
        [SerializeField] private float afterActionDelay = 1;
        [SerializeField] private UnityEvent onEat;

        public override IEnumerator Do(Pawn pawn)
        {
            onEat.Invoke();
            Printer.Log($"MAMMA MIA THE PLAYER HAS EATEN A WHOLE PI OF PIZZA");
            gameObject.SetActive(false);
            yield return new WaitForSeconds(afterActionDelay);
        }
    }
}