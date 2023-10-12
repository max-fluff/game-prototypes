using System;
using UnityEngine;

namespace MaxFluff.Prototypes
{
    public class Trigger : MonoBehaviour
    {
        public event Action<Collider> OnTriggerEntered;
        public event Action<Collider> OnTriggerExited;
        private void OnTriggerEnter(Collider other) => OnTriggerEntered?.Invoke(other);
        private void OnTriggerExit(Collider other) => OnTriggerExited?.Invoke(other);
    }
}