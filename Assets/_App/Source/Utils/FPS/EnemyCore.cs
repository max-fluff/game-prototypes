using System;
using UnityEngine;

namespace MaxFluff.Prototypes
{
    public class EnemyCore : MonoBehaviour, IZappableObject
    {
        public event Action OnZapped;

        public void Zap()
        {
            OnZapped?.Invoke();
        }
    }
}