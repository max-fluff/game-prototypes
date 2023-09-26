using UnityEngine;

namespace MaxFluff.Prototypes
{
    public class Rotator : MonoBehaviour
    {
        [SerializeField] private float RotationSpeed;

        private void Update()
        {
            transform.Rotate(Vector3.forward, RotationSpeed * Time.deltaTime);
        }
    }
}