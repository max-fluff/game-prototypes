using Cysharp.Threading.Tasks;
using UnityEngine;

namespace MaxFluff.Prototypes
{
    public class Sender : MonoBehaviour
    {
        [SerializeField] private Vector3 force;
        private AudioSource audioSource;

        private void Awake()
        {
            audioSource = GetComponent<AudioSource>();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
                SendObject(other.gameObject.GetComponentInParent<Rigidbody>()).Forget();
        }

        private async UniTask SendObject(Rigidbody rigidbody)
        {
            audioSource.PlayOneShot(audioSource.clip);
            for (var i = 0; i < 10; i++)
            {
                rigidbody.AddForce(force / 10);
                await UniTask.WaitForFixedUpdate();
            }
        }
    }
}