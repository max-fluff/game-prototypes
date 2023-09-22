using UnityEngine;

namespace MaxFluff.Prototypes
{
    public class GameContext : SceneContextBase
    {
        [Header("Workspace")]
        public CameraView UICamera;
        public RaycastView RaycastView;
        public CommonUIContext UI;
    }
}