using UnityEngine;

namespace MaxFluff.Prototypes
{
    public sealed class LightView : ViewBase
    {
        public Light[] Lights;
    }

    public sealed class LightPresenter : PresenterBase<LightView>
    {
        public LightPresenter(LightView view) : base(view)
        {
        }

        public void SetShadowsMode(bool castShadows)
        {
            foreach (var light in _view.Lights)
                light.shadows = castShadows
                    ? LightShadows.Soft
                    : LightShadows.None;
        }
    }
}