using Eflatun.SceneReference;
using UnityEngine;

namespace MaxFluff.Prototypes
{
    [CreateAssetMenu(fileName = "AppScenes", menuName = "Config/AppScenes", order = 0)]
    public sealed class AppScenes : ScriptableObject
    {
        public SceneReference EmptyScene;
        public SceneReference StartScreen;
        public SceneReference PlatformerScene;
        public SceneReference PuzzleScene;
        public SceneReference RaceScene;
        public SceneReference DialogsScene;
        public SceneReference FPSScene;
        public SceneReference TycoonState;
        public SceneReference NarrativeScene;
        public SceneReference TBSScene;
    }
}