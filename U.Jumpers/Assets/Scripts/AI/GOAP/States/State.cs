using System;
using UnityEngine;

namespace Core.States
{
    [Serializable]
    public class State
    {
        [SerializeField]
        public string id;
        [SerializeField]
        public object value;
    }
}