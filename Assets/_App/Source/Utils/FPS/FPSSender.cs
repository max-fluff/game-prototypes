using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace MaxFluff.Prototypes
{
    public class FPSSender : MonoBehaviour, IZappableObject
    {
        [SerializeField] private Vector3 force;
        [SerializeField] private float ResetTime;
        [SerializeField] private List<ParticleSystem> Particles;

        private bool _isZapped;
        private float _timeFromZap;

        public void Zap()
        {
            _timeFromZap = 0f;

            if (_isZapped) return;

            _isZapped = true;
            ZapAsync().Forget();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!_isZapped) return;

            var rb = other.GetComponentInParent<Rigidbody>();
            if (rb)
                SendAsync(rb).Forget();
        }

        private async UniTask SendAsync(Rigidbody rb)
        {
            for (var i = 0; i < 10; i++)
            {
                rb.AddForce(force / 10);
                await UniTask.WaitForFixedUpdate();
            }
        }

        private async UniTask ZapAsync()
        {
            foreach (var particle in Particles)
                particle.gameObject.SetActive(true);

            await CountDownUntilReset();
            _isZapped = false;

            foreach (var particle in Particles)
                particle.gameObject.SetActive(false);
        }

        private async UniTask CountDownUntilReset()
        {
            _timeFromZap = 0f;
            while (_timeFromZap < ResetTime)
            {
                _timeFromZap += Time.deltaTime;
                await UniTask.NextFrame();
            }
        }
    }
}