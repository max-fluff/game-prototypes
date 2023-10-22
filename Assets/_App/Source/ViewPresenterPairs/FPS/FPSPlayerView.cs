using Cysharp.Threading.Tasks;
using UnityEngine;

namespace MaxFluff.Prototypes.FPS
{
    public class FPSPlayerView : PlayerView
    {
        public Rigidbody Rigidbody;
        public Transform ShotSource;
        public LineRenderer ShotLinePrefab;
    }

    public class FPSPlayerPresenter : PlayerPresenter<FPSPlayerView>
    {
        public Rigidbody Rigidbody => _view.Rigidbody;

        public FPSPlayerPresenter(FPSPlayerView view) : base(view)
        {
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