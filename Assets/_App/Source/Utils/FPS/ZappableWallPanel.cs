using Cysharp.Threading.Tasks;
using UnityEngine;

namespace MaxFluff.Prototypes.FPS
{
    public class ZappableWallPanel : MonoBehaviour, IZappableObject
    {
        public float ResetTime;
        public float MovingTime;

        public Vector3 ActivePosition;
        public Rigidbody Rigidbody;

        private float _timeFromZap;
        private float _speed;
        private float _distanceTotal;
        private Vector3 _initPosition;
        private bool _isZapped;

        public void Zap()
        {
            _timeFromZap = 0f;

            if (_isZapped) return;

            _isZapped = true;
            ZapAsync().Forget();
        }

        private async UniTask ZapAsync()
        {
            await MoveToActivePosition();
            await CountDownUntilReset();
            _isZapped = false;
            await MoveToInitPosition();
        }

        private async UniTask MoveToActivePosition()
        {
            var dist = Vector3.Distance(_initPosition, Rigidbody.transform.position);
            while (Vector3.Distance(Rigidbody.transform.position, ActivePosition) > 0.01f)
            {
                await UniTask.NextFrame();

                var newPosition = Vector3.Lerp(
                    _initPosition,
                    ActivePosition,
                    (dist + _speed * Time.deltaTime) / _distanceTotal);

                Rigidbody.MovePosition(newPosition);

                dist = Vector3.Distance(_initPosition, Rigidbody.transform.position);

                if (dist > _distanceTotal)
                {
                    Rigidbody.MovePosition(ActivePosition);
                    return;
                }
            }

            Rigidbody.MovePosition(ActivePosition);
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

        private async UniTask MoveToInitPosition()
        {
            var dist = Vector3.Distance(ActivePosition, Rigidbody.transform.position);
            while (Vector3.Distance(Rigidbody.transform.position, _initPosition) > 0.01f)
            {
                await UniTask.NextFrame();

                if (_isZapped)
                    return;

                var newPosition = Vector3.Lerp(
                    ActivePosition,
                    _initPosition,
                    (dist + _speed * Time.deltaTime) / _distanceTotal);

                Rigidbody.MovePosition(newPosition);

                dist = Vector3.Distance(ActivePosition, Rigidbody.transform.position);

                if (dist > _distanceTotal)
                {
                    Rigidbody.MovePosition(_initPosition);
                    return;
                }
            }

            Rigidbody.MovePosition(_initPosition);
        }

        private void Awake()
        {
            _initPosition = Rigidbody.transform.position;

            _distanceTotal = Vector3.Distance(_initPosition, ActivePosition);
            _speed = _distanceTotal / MovingTime;
        }
    }
}