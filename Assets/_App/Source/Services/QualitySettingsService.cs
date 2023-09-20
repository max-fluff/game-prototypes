using System;
using UnityEngine;

namespace MaxFluff.Prototypes
{
    public sealed class QualitySettingsService
    {
        public event Action<int> OnQualitySettingsUpdated;
        public event Action<int> OnVSyncUpdated; 

        public void UpdateQualitySettings(int index)
        {
            QualitySettings.SetQualityLevel(index);
            OnQualitySettingsUpdated?.Invoke(index);
        }

        public void UpdateVSyncCount(int index)
        {
            QualitySettings.vSyncCount = index;
            OnVSyncUpdated?.Invoke(index);
        }

        public int GetQualityPresetIndex()
        {
            return QualitySettings.GetQualityLevel();
        }

        public int GetVSyncCount()
        {
            return QualitySettings.vSyncCount;
        }
    }
}