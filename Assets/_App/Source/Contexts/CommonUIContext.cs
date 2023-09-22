using System;
using UnityEngine;

namespace MaxFluff.Prototypes
{
    [Serializable]
    public sealed class CommonUIContext
    {
        [Header("Windows")]
        public WindowsOrganizerView WindowsOrganizer;
        public LoadingWindowView LoadingWindow;
    }
}