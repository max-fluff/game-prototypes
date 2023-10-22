using UnityEngine;

namespace MaxFluff.Prototypes.FPS
{
    public class FPSPlayerView : PlayerView
    {
        public Rigidbody Rigidbody;
    }

    public class FPSPlayerPresenter : PlayerPresenter<FPSPlayerView>
    {
        public Rigidbody Rigidbody => _view.Rigidbody;

        public FPSPlayerPresenter(FPSPlayerView view) : base(view)
        {
        }
    }
}