using System;
using UnityEngine;

namespace MaxFluff.Prototypes
{
    public class PlatformerPlayerView : PlayerView
    {
        public Rigidbody Rigidbody;

        public GameObject Square;
        public GameObject Circle;
        public GameObject Triangle;
        public AudioSource AudioSource;

        public PhysicMaterial CirclePhysicsMaterial;
        
        public AudioClip Coin;
        public AudioClip State;
        public AudioClip Full;
        public AudioClip Jump;
    }

    public class PlatformerPlayerPresenter : PlayerPresenter<PlatformerPlayerView>
    {
        private PlatformerPlayerState _state = PlatformerPlayerState.None;

        public Rigidbody Rigidbody => _view.Rigidbody;

        private int _energy;
        public const int MaxEnergy = 300;

        public event Action<float> OnEnergyAmountUpdate;

        public PlatformerPlayerState State
        {
            get => _state;
            set
            {
                Rigidbody.constraints = value switch
                {
                    PlatformerPlayerState.Triangle => RigidbodyConstraints.FreezeRotation |
                                                      RigidbodyConstraints.FreezePositionZ,
                    PlatformerPlayerState.Square => RigidbodyConstraints.FreezeRotation |
                                                    RigidbodyConstraints.FreezePositionZ,
                    PlatformerPlayerState.Circle => RigidbodyConstraints.FreezePositionZ |
                                                    RigidbodyConstraints.FreezeRotationX |
                                                    RigidbodyConstraints.FreezeRotationY,
                    _ => Rigidbody.constraints
                };

                Rigidbody.mass = value switch
                {
                    PlatformerPlayerState.Square => 0.3f,
                    PlatformerPlayerState.Triangle => 20f,
                    PlatformerPlayerState.Circle => 20f,
                    _ => Rigidbody.mass
                };

                if (value == PlatformerPlayerState.Triangle)
                {
                    Rigidbody.velocity = Vector3.zero;
                    Rigidbody.angularVelocity = Vector3.zero;
                }

                Rigidbody.isKinematic = false;

                Transform.rotation = Quaternion.identity;

                _view.Square.SetActive(value == PlatformerPlayerState.Square);
                _view.Circle.SetActive(value == PlatformerPlayerState.Circle);
                _view.Triangle.SetActive(value == PlatformerPlayerState.Triangle);
                _state = value;
            }
        }
        
        public int Energy
        {
            get => _energy;
            set
            {
                _energy = value > MaxEnergy ? MaxEnergy : value;
                OnEnergyAmountUpdate?.Invoke((float)Energy / MaxEnergy);
            }
        }

        public void PlayCoinSound() =>
            _view.AudioSource.PlayOneShot(_view.Coin);
        
        public void PlayFullSound() =>
            _view.AudioSource.PlayOneShot(_view.Full);
        
        public void PlayStateSound() =>
            _view.AudioSource.PlayOneShot(_view.State);

        public void PlayJumpSound() =>
            _view.AudioSource.PlayOneShot(_view.Jump);

        public PlatformerPlayerPresenter(PlatformerPlayerView view) : base(view)
        {
            _view.CirclePhysicsMaterial.bounciness = 0;
        }
    }

    public enum PlatformerPlayerState
    {
        Square,
        Circle,
        Triangle,
        None
    }
}