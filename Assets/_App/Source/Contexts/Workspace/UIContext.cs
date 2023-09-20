using System;
using UnityEngine;

namespace MaxFluff.Prototypes
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