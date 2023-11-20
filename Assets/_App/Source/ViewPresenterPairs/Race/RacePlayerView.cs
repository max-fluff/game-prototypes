using DG.Tweening;
using UnityEngine;

namespace MaxFluff.Prototypes
{
    public class RacePlayerView : PlayerView
    {
        public Rigidbody Rigidbody;

        public AudioSource AudioSource;
    }

    public class RacePlayerPresenter : PlayerPresenter<RacePlayerView>
    {
        private Quaternion _initRotation;
        private Vector3 _initPosition;

        public Vector3 Position
        {
            get => _view.transform.position;
            set => _view.transform.position = value;
        }

        public Rigidbody Rigidbody => _view.Rigidbody;

        public Vector3 InitPosition => _initPosition;

        public RacePlayerPresenter(RacePlayerView view) : base(view)
        {
            _initPosition = _view.transform.position;
            _initRotation = _view.transform.rotation;
        }

        public void ResetPlayer()
        {
            _view.transform.DOKill();
            Rigidbody.velocity = Vector3.zero;
            Rigidbody.angularVelocity = Vector3.zero;
            _view.transform.position = _initPosition;
            _view.transform.rotation = _initRotation;
        }
    }
}