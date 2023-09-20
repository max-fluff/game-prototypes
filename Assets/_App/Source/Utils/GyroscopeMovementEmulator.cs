using UnityEngine;

namespace Omega.Kulibin
{
    public sealed class GyroscopeMovementEmulator : MonoBehaviour
    {
        private const float DEGREES_PER_UNIT_PER_SECOND = 5f;

        [SerializeField] private Rigidbody rigidbody;
        [SerializeField] private Transform visualTransform;
        [SerializeField, Range(0, 90)] private float maxRotationDegree = 30;
        private Vector3 _speed;
        private float _rotationSpeed;

        public void Reset()
        {
            _speed.z = 0;
            _speed.x = 0;
            _speed.y = 0;
            _rotationSpeed = 0;
            rigidbody.velocity = Vector3.zero;
            rigidbody.angularVelocity = Vector3.zero;
        }

        public bool GravitationActive
        {
            get => rigidbody.useGravity;
            set => rigidbody.useGravity = value;
        }

        public float SpeedX { get => _speed.z; set => _speed.z = value; }
        public float SpeedY { get => _speed.x; set => _speed.x = value; }
        public float SpeedZ { get => _speed.y; set => _speed.y = value; }
        public float RotationSpeed { get => _rotationSpeed; set => _rotationSpeed = value; }
        
        private void LateUpdate() => VisualTransformRotation();

        private void VisualTransformRotation()
        {
            var deltaPositionLocal = visualTransform.InverseTransformDirection(rigidbody.velocity);
            var speedX = deltaPositionLocal.x / Time.deltaTime;
            var speedZ = deltaPositionLocal.z / Time.deltaTime;
            var degreesX = speedX * DEGREES_PER_UNIT_PER_SECOND;
            var degreesZ = speedZ * DEGREES_PER_UNIT_PER_SECOND;
            degreesX = Mathf.Clamp(degreesX, -maxRotationDegree, maxRotationDegree);
            degreesZ = Mathf.Clamp(degreesZ, -maxRotationDegree, maxRotationDegree);
            var targetRotation =
                Quaternion.Euler(degreesZ, visualTransform.localRotation.eulerAngles.y, -degreesX);
            visualTransform.localRotation =
                Quaternion.Lerp(visualTransform.localRotation, targetRotation, 6 * Time.deltaTime);
        }

        private void FixedUpdate()
        {
            if (GravitationActive) 
                return;
            
            rigidbody.velocity = rigidbody.transform.TransformDirection(_speed * Time.fixedDeltaTime);
            rigidbody.angularVelocity = new Vector3(0, _rotationSpeed * Time.fixedDeltaTime, 0);
        }
    }
}