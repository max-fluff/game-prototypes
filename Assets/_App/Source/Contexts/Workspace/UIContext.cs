using System;
using UnityEngine;

namespace Omega.Kulibin
{
    [Serializable]
    public sealed class UIContext
    {
        [Header("Windows")]
        public WindowsOrganizerView WindowsOrganizer;
        public SettingsWindowView SettingsWindow;
        public LoadingWindowView LoadingWindow;
    }
}