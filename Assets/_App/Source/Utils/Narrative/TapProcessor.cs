using System.Collections.Generic;
using Lean.Transition;
using UnityEngine;
using UnityEngine.Events;
using Random = System.Random;

namespace MaxFluff.Prototypes
{
    public class TapProcessor : MonoBehaviour
    {
        [SerializeField] private AudioSource tapSound;
        [SerializeField] private List<LeanPlayer> transitionEffects = new List<LeanPlayer>();

        public UnityEvent OnTappped;
        public UnityEvent OnDraggedRight;

        private Vector3 _cachedMousePosition;
        private bool _movedRight;

        private readonly Random _random = new Random();

        private void OnMouseDown()
        {
            if (enabled == false) return;

            if (tapSound)
                tapSound.PlayOneShot(tapSound.clip);
            OnTappped?.Invoke();

            if (transitionEffects.Count > 0)
                transitionEffects[_random.Next(transitionEffects.Count)].Begin();
        }

        private void Update()
        {
            _movedRight = false;
            var mousePosition = Input.mousePosition;
            if ((mousePosition - _cachedMousePosition).x > 0.1f)
                _movedRight = true;

            _cachedMousePosition = mousePosition;
        }

        private void OnMouseDrag()
        {
            if (_movedRight)
                OnDraggedRight?.Invoke();
        }
    }
}