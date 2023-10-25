using Cysharp.Threading.Tasks;
using UnityEngine;
using Object = UnityEngine.Object;

namespace MaxFluff.Prototypes
{
    public class ZapRetranslator : MonoBehaviour, IZappableObject
    {
        public LineRenderer ShotLinePrefab;
        public ZapRetranslatorArea Area;

        private bool _wasZappedInCurrentFrame;


        public void Zap()
        {
            if (_wasZappedInCurrentFrame) return;

            _wasZappedInCurrentFrame = true;

            foreach (var zappable in Area.ZappableObjectsInRange)
            {
                if (!(Component)zappable)
                {
                    continue;
                }

                var targetPosition = ((Component)zappable).transform.position;
                if (Physics.Raycast(transform.position, targetPosition - transform.position, out var hit,
                        Vector3.Distance(targetPosition, transform.position)))
                {
                    var zappableInParent = hit.collider.GetComponentInParent<IZappableObject>();
                    if (zappableInParent is null || zappableInParent != zappable)
                        continue;
                }

                zappable.Zap();
                VisualizeShot(targetPosition);
            }

            UnzapAtTheEndOfFrame().Forget();
        }

        private async UniTask UnzapAtTheEndOfFrame()
        {
            await UniTask.Yield(PlayerLoopTiming.LastPostLateUpdate);
            _wasZappedInCurrentFrame = false;
        }

        private void VisualizeShot(Vector3 hit)
        {
            var lineRenderer = Instantiate(ShotLinePrefab);
            lineRenderer.positionCount = 2;
            lineRenderer.SetPositions(new[]
            {
                transform.position, hit
            });

            DestroyShotDelayed(lineRenderer.gameObject).Forget();
        }

        private async UniTask DestroyShotDelayed(Object shotGameObject)
        {
            await UniTask.Delay(500);
            Destroy(shotGameObject);
        }
    }
}