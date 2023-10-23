using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Object = UnityEngine.Object;

namespace MaxFluff.Prototypes.FPS
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
                zappable.Zap();
                VisualizeShot(((Component)zappable).transform.position);
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