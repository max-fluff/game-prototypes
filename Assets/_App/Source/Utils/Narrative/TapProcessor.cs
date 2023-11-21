using System.Collections.Generic;
using Lean.Transition;
using UnityEngine;
using UnityEngine.Events;

namespace MaxFluff.Prototypes.Narrative
{
    public class TapProcessor : MonoBehaviour
    {
        [SerializeField] private AudioSource tapSound;
        [SerializeField] private List<LeanPlayer> transitionEffects = new List<LeanPlayer>();

        public UnityEvent OnTappped;

        private void OnMouseDown()
        {
            tapSound?.PlayOneShot(tapSound.clip);
            OnTappped?.Invoke();
            transitionEffects.ForEach(te => te.Begin());
        }
    }
}