using Eflatun.SceneReference;
using UnityEngine;

namespace MaxFluff.Prototypes
{
    [CreateAssetMenu(fileName = "AppScenes", menuName = "Config/AppScenes", order = 0)]
    public sealed class AppScenes : ScriptableObject
    {
        public SceneReference EmptyScene;
        public SceneReference StartScreen;
        public SceneReference Game;
    }
}