using System;
using System.ComponentModel;
using System.Globalization;
using System.Reflection;
using Newtonsoft.Json;
using Omega.Lib.Text;
using Omega.RemoteConfig;
using UnityEngine;

namespace Omega.Kulibin
{
    public static class AppEntryPoint
    {
        public static App App { get; set; }

        [RuntimeInitializeOnLoadMethod]
        private static void EntryPoint()
        {
            var startupConfiguration = GetStartupConfiguration();
            var runtimeConfiguration = GetRuntimeConfiguration(startupConfiguration);

            if (runtimeConfiguration.StartupConfiguration.RemoteConfigEnabled)
            {
                Debug.Log(new RichString(enableRichTags: Application.isEditor, Color.yellow,
                    "Remote configuration feature is used"));
            }

            App = new App(runtimeConfiguration);

            var configurationJson = JsonConvert.SerializeObject(runtimeConfiguration, Formatting.Indented);

            var kulibinColor = new Color32(0x34, 0x27, 0xFF, 0xFF);

            var message = new RichString(enableRichTags: Application.isEditor,
                (kulibinColor, "■"), " App ", (Color.green, "✔ Started"), "\n", configurationJson);

            Debug.Log(message);
        }

        private static AppStartupConfiguration GetStartupConfiguration()
        {
            try
            {
                return AppStartupConfigurationProvider.LoadConfigurationFromDataPath();
            }
            catch (Exception e)
            {
                Debug.LogError("Cant load configuration from data path. Default configuration will be used");
                Debug.LogException(e);

                return AppStartupConfigurationProvider.GetDefaultConfiguration();
            }
        }

        private static AppRuntimeConfiguration GetRuntimeConfiguration(AppStartupConfiguration appStartupConfiguration)
        {
            var runtimeConfiguration = appStartupConfiguration.DefaultRuntimeConfiguration;
            runtimeConfiguration.StartupConfiguration = appStartupConfiguration;
            return runtimeConfiguration;
        }

        private static AppRuntimeConfiguration GetConfigurationFromRemote(Configuration configuration,
            AppRuntimeConfiguration defaultConfiguration)
        {
            object runtimeConfiguration = defaultConfiguration;
            var configurationFields =
                runtimeConfiguration.GetType().GetFields(BindingFlags.Public | BindingFlags.Instance);

            foreach (var fieldInfo in configurationFields)
            {
                var propertyName = fieldInfo.Name;
                var propertyType = fieldInfo.FieldType;
                if (configuration.TryGetPropertyValue(propertyName, out var propertyValue))
                {
                    try
                    {
                        var descriptor = TypeDescriptor.GetConverter(propertyType);
                        var value = descriptor.ConvertFromString(null, CultureInfo.InvariantCulture, propertyValue);
                        fieldInfo.SetValue(runtimeConfiguration, value);
                    }
                    catch (Exception e)
                    {
                        Debug.LogException(e);
                    }
                }
            }

            return (AppRuntimeConfiguration) runtimeConfiguration;
        }
    }
}