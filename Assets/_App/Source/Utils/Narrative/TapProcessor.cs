using System.Collections.Generic;
using Lean.Transition;
using UnityEngine;
using UnityEngine.Events;

namespace MaxFluff.Prototypes
{
    public class TapProcessor : MonoBehaviour
    {
        [SerializeField] private AudioSource tapSound;
        [SerializeField] private List<LeanPlayer> transitionEffects = new List<LeanPlayer>();

        public UnityEvent OnTappped;

        private void OnMouseDown()
        {
            if (enabled == false) return;
            
            if (tapSound)
                tapSound.PlayOneShot(tapSound.clip);
            OnTappped?.Invoke();
            transitionEffects.ForEach(te => te.Begin());
        }
    }
}