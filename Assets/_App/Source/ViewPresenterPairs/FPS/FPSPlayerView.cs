using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Object = UnityEngine.Object;

namespace MaxFluff.Prototypes.FPS
{
    public class FPSPlayerView : PlayerView
    {
        public Rigidbody Rigidbody;
        public Transform ShotSource;
        public LineRenderer ShotLinePrefab;

        public event Action<float> OnDamaged;

        public void Damage(float value) => OnDamaged?.Invoke(value);
    }

    public class FPSPlayerPresenter : PlayerPresenter<FPSPlayerView>
    {
        public Rigidbody Rigidbody => _view.Rigidbody;

        public float Health { get; private set; } = 100f;

        public event Action<float> OnHealthUpdate;

        public FPSPlayerPresenter(FPSPlayerView view) : base(view)
        {
            _view.OnDamaged += ProcessDamage;
        }

        private void ProcessDamage(float damage)
        {
            Health -= damage;
            Health = Mathf.Clamp(Health, 0, 100);
            OnHealthUpdate?.Invoke(Health);
        }

        public void VisualizeShot(Vector3 hit)
        {
            var lineRenderer = Object.Instantiate(_view.ShotLinePrefab);
            lineRenderer.positionCount = 2;
            lineRenderer.SetPositions(new[]
            {
                _view.ShotSource.position, hit
            });

            DestroyShotDelayed(lineRenderer.gameObject).Forget();
        }

        private async UniTask DestroyShotDelayed(Object shotGameObject)
        {
            await UniTask.Delay(500);
            Object.Destroy(shotGameObject);
        }
    }
}