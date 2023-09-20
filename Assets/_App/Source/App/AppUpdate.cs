using UnityEngine;

namespace Omega.Kulibin
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