using UnityEngine;

namespace MaxFluff.Prototypes
{
    public class MovingPlatform : MonoBehaviour
    {
        [SerializeField] private Vector3 finalPosition;
        [SerializeField] private float lengthInSeconds;
        [SerializeField] private Rigidbody rigidbody;

        private bool _isReversing;
        private float _timeFromStart;
        private Vector3 _initPosition;

        private void Awake()
        {
            _initPosition = transform.position;
        }

        private void Update()
        {
            _timeFromStart += Time.deltaTime;

            var newPosition = _isReversing
                ? Vector3.Lerp(finalPosition, _initPosition,
                    Mathf.Clamp(_timeFromStart / lengthInSeconds, 0f, lengthInSeconds))
                : Vector3.Lerp(_initPosition, finalPosition,
                    Mathf.Clamp(_timeFromStart / lengthInSeconds, 0f, lengthInSeconds));

            rigidbody.MovePosition(newPosition);

            if (_timeFromStart >= lengthInSeconds)
            {
                _timeFromStart = 0f;
                _isReversing = !_isReversing;
            }
        }
    }
}