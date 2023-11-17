using UnityEngine;
using UnityEngine.Events;

namespace MaxFluff.Prototypes
{
    [RequireComponent(typeof(BoxCollider))]
    public class PlayerTrigger : MonoBehaviour
    {
        public UnityEvent OnPlayerEnter;
        public UnityEvent OnPlayerExit;

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