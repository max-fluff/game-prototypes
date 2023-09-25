using System;
using UnityEngine;

namespace MaxFluff.Prototypes
{
    public class PlayerTrigger : MonoBehaviour
    {
        public event Action OnPlayerEnter;
        public event Action OnPlayerExit;

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
                OnPlayerEnter?.Invoke();
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
                OnPlayerExit?.Invoke();
        }
    }
}