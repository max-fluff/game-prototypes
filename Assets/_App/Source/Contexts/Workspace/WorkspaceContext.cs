using UnityEngine;

namespace Omega.Kulibin
{
    public sealed class WorkspaceContext : SceneContextBase
    {
        [Header("Workspace")]
        public CameraView UICamera;
        public RaycastView RaycastView;
        public UIContext UI;
    }
}