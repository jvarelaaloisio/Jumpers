using System.Collections;
using UnityEngine;

namespace Core
{
    public class EnableAfter : MonoBehaviour
    {
        [SerializeField] private float delay;
        [SerializeField] private MonoBehaviour componentToEnable;
    
        private IEnumerator Start()
        {
            yield return new WaitForSeconds(delay);
            componentToEnable.enabled = true;
        }
    }
}
