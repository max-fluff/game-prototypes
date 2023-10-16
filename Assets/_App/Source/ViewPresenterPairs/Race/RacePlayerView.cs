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

        public Rigidbody Rigidbody => _view.Rigidbody;

        public RacePlayerPresenter(RacePlayerView view) : base(view)
        {
            _initPosition = _view.transform.position;
            _initRotation = _view.transform.rotation;
        }

        public void ResetPlayer()
        {
            Rigidbody.velocity = Vector3.zero;
            Rigidbody.angularVelocity = Vector3.zero;
            _view.transform.position = _initPosition;
            _view.transform.rotation = _initRotation;
        }
    }
}