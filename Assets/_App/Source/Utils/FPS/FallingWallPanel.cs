using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Random = System.Random;

namespace MaxFluff.Prototypes
{
    public class FallingWallPanel : MonoBehaviour, IZappableObject
    {
        public Rigidbody Rigidbody;
        private bool _isZapped;

        public void Zap()
        {
            if (_isZapped) return;

            var rand = new Random();
            Rigidbody.isKinematic = false;
            Rigidbody.useGravity = true;
            var angularVelocity = new Vector3(rand.Next(), rand.Next(), rand.Next());
            var delta = angularVelocity.magnitude / 50;
            angularVelocity /= delta;
            Rigidbody.angularVelocity = angularVelocity;

            DestroyAfterDelay().Forget();
        }

        private async UniTask DestroyAfterDelay()
        {
            await UniTask.Delay(500);
            Destroy(Rigidbody.gameObject);
        }
    }
}