using UnityEngine;

namespace MaxFluff.Prototypes
{
    public abstract class GameContext : SceneContextBase
    {
        [Header("Workspace")] public RaycastView RaycastView;
        public CommonUIContext UI;
    }
}