using System.IO;
using Newtonsoft.Json;
using UnityEngine;

namespace Omega.Kulibin
{
    public static class AppStartupConfigurationProvider
    {
        public static string PathToDeveloperMarker => Application.persistentDataPath + "/config.txt";
        public static string PathToStartupConfigurationFile => Path.Combine(Application.dataPath, "startup.json");
        
        public static AppStartupConfiguration LoadConfigurationFromDataPath()
        {
            var startupConfigurationJson = File.ReadAllText(PathToStartupConfigurationFile);

            var startupConfiguration = JsonConvert.DeserializeObject<AppStartupConfiguration>(startupConfigurationJson);

            return startupConfiguration;
        }

        public static AppStartupConfiguration GetDefaultConfiguration()
            => new AppStartupConfiguration();
    }
}