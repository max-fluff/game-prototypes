using UnityEngine;

namespace Omega.Kulibin
{
    public sealed class StartScreenContext : SceneContextBase
    {
        [Header("Views")]
        public StartScreenView StartScreen;
        public WindowsOrganizerView WindowsOrganizer;
        public RaycastView RaycastView;
        
        [Header("Windows")]
        public LoadingWindowView LoadingWindow;
    }
}