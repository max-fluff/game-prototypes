using UnityEngine;

namespace MaxFluff.Prototypes
{
    public sealed class GameContext : SceneContextBase
    {
        [Header("Workspace")]
        public CameraView UICamera;
        public RaycastView RaycastView;
        public UIContext UI;
    }
}