using System;
using UnityEngine;

namespace Omega.Kulibin
{
    public class TriggerDetector : MonoBehaviour
    {
        [SerializeField] private LayerMask _triggerMask;
        
        public event Action<Collider> OnEnter;
        public event Action<Collider> OnExit;

        private void OnTriggerEnter(Collider otherCollider)
        {
            var colliderLayer = 1 << otherCollider.gameObject.layer;
            if ((colliderLayer & _triggerMask) == colliderLayer)
                OnEnter?.Invoke(otherCollider);
        }

        private void OnTriggerExit(Collider otherCollider)
        {
            var colliderLayer = 1 << otherCollider.gameObject.layer;
            if ((colliderLayer & _triggerMask) == colliderLayer)
                OnExit?.Invoke(otherCollider);
        }
    }
}