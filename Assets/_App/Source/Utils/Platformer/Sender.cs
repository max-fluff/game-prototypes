using UnityEngine;

namespace MaxFluff.Prototypes
{
    public class Sender: MonoBehaviour
    {
        [SerializeField] private Vector3 Force;
        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
                other.gameObject.GetComponentInParent<Rigidbody>().AddForce(Force);
        }
    }
}