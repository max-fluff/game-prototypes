using UnityEngine;

namespace MaxFluff.Prototypes
{
    public sealed class AppUpdate : MonoBehaviour
    {
        public App App { get; set; }
        
        private void Update()
        {
            App.Run();
        }

        private void OnApplicationQuit()
        {
            App.OnApplicationQuit();
        }
    }
}