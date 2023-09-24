using UnityEngine;

namespace MaxFluff.Prototypes
{
    public class PlatformerPlayerView : TransformView
    {
        public Rigidbody Rigidbody;

        public GameObject Square;
        public GameObject Circle;
    }

    public class PlatformerPlayerPresenter : TransformPresenter<PlatformerPlayerView>
    {
        private PlatformerPlayerState _state;

        public Rigidbody Rigidbody => _view.Rigidbody;

        public PlatformerPlayerState State
        {
            get => _state;
            set
            {
                Rigidbody.constraints = value switch
                {
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
                    PlatformerPlayerState.Circle => 20f,
                    _ => Rigidbody.mass
                };
                
                Transform.rotation = Quaternion.identity;

                _view.Square.SetActive(value == PlatformerPlayerState.Square);
                _view.Circle.SetActive(value == PlatformerPlayerState.Circle);
                _state = value;
            }
        }

        public PlatformerPlayerPresenter(PlatformerPlayerView view) : base(view)
        {
        }
    }

    public enum PlatformerPlayerState
    {
        Square,
        Circle,
        Triangle
    }
}