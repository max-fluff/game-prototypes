using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using MaxFluff.Prototypes.FPS;
using UnityEngine;
using Random = System.Random;

namespace MaxFluff.Prototypes
{
    public class Enemy : MonoBehaviour
    {
        [SerializeField] private EnemyCore core;
        [SerializeField] private Rigidbody rigidbody;
        [SerializeField] private float disabilityTime;

        private FPSPlayerView _player;
        private float _timeFromZap;
        private bool _isZapped;
        private static Random _random;

        private void Awake()
        {
            core.OnZapped += TemporaryDisable;
            _player = FindObjectOfType<FPSPlayerView>();
            _random ??= new Random();

            Movement().Forget();
        }

        private void TemporaryDisable()
        {
            _timeFromZap = 0;
            if (_isZapped)
                return;
            _isZapped = true;

            rigidbody.isKinematic = false;
            rigidbody.useGravity = true;

            DestroyAfterTimeout().Forget();
        }

        private async UniTask Movement()
        {
            while (!_isZapped)
            {
                var dir = (_player.transform.position - transform.position).normalized;
                var isInView = !Physics.Raycast(
                    transform.position + dir * 2f,
                    dir,
                    (_player.transform.position - (transform.position + dir * 2f)).magnitude,
                    LayerMask.GetMask("Default"));

                if (isInView)
                    transform.LookAt(_player.transform);
                else if (Physics.Raycast(transform.position + transform.forward * 1.5f, transform.forward, 5f))
                {
                    var addedRotation = Vector3.right * _random.Next(-180, 180) + Vector3.up * _random.Next(-180, 180);

                    transform.DORotate(addedRotation, .5f, RotateMode.LocalAxisAdd);
                    await UniTask.Delay(500);
                }

                rigidbody.MovePosition(transform.position + transform.forward * 55f * Time.deltaTime);

                await UniTask.NextFrame();
            }
        }

        private async UniTask DestroyAfterTimeout()
        {
            while (_timeFromZap < disabilityTime)
            {
                _timeFromZap += Time.deltaTime;
                await UniTask.NextFrame();
            }

            _isZapped = false;
            Destroy(gameObject);
        }

        private void OnDestroy()
        {
            _isZapped = true;
        }
    }
}