using System;
using UnityEngine;

namespace MaxFluff.Prototypes
{
    public class EnemyDamageCollider : MonoBehaviour
    {
        public event Action<Collider> OnEnter;

        private void OnTriggerEnter(Collider other)
        {
            OnEnter?.Invoke(other);
        }
    }
}