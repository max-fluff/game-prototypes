using System;
using UnityEngine;

namespace MaxFluff.Prototypes
{
    public abstract class ViewBase : MonoBehaviour
    {
        public event Action OnDestroying;
        private void OnDestroy() => OnDestroying?.Invoke();
    }
}