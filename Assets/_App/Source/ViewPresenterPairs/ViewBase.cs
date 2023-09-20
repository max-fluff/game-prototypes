using System;
using UnityEngine;

namespace Omega.Kulibin
{
    public abstract class ViewBase : MonoBehaviour
    {
        public event Action OnDestroying;
        private void OnDestroy() => OnDestroying?.Invoke();
    }
}