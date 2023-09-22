using UnityEngine;

namespace MaxFluff.Prototypes
{
    [CreateAssetMenu(fileName = FILE_NAME, menuName = "Config/UnityConfigs", order = 0)]
    public sealed class UnityConfigs : ScriptableObject
    {
        private const string FILE_NAME = "UnityConfigs";
        
        public AppScenes AppScenes;
        public CursorsConfig Cursors;
        public GamesList Games;

        public static UnityConfigs Load()
        {
            return Resources.Load<UnityConfigs>(FILE_NAME);
        }
    }
}