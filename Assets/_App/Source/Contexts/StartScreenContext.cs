using UnityEngine;

namespace MaxFluff.Prototypes
{
    public sealed class StartScreenContext : SceneContextBase
    {
        [Header("Views")]
        public StartScreenView StartScreen;
        public WindowsOrganizerView WindowsOrganizer;
        public RaycastView RaycastView;
        public GamesListView GamesListView;
        
        [Header("Windows")]
        public LoadingWindowView LoadingWindow;
        public QuitWindowView QuitWindow;
    }
}