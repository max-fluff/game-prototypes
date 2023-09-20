using System.IO;
using UnityEditor;

namespace Omega.Kulibin.Editor
{
    [InitializeOnLoad]
    public class MarkAsDeveloperTool
    {
        static MarkAsDeveloperTool()
        {
            if (!File.Exists(AppStartupConfigurationProvider.PathToDeveloperMarker))
                File.Create(AppStartupConfigurationProvider.PathToDeveloperMarker);
        }
    }
}