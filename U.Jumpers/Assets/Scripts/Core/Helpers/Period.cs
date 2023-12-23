using System;
using System.Collections;
using UnityEngine;

namespace Core.Helpers
{
    [Serializable]
    public class Period
    {
        [Tooltip("If true, " + nameof(period) + " field will be ignored and will return yield null")]
        [SerializeField] private bool periodIsFrame;

        [Tooltip("Period (s). Ignored if " + nameof(periodIsFrame) + " is true")]
        [Min(float.Epsilon)]
        [SerializeField] private float period = 1;
        
        public IEnumerator Wait()
        {
            if (periodIsFrame)
                yield return null;
            yield return new WaitForSeconds(period);
        }
    }
}