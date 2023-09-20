using System;
using UnityEngine;

namespace MaxFluff.Prototypes
{
    public sealed class PlayerPrefsService
    {
        private string _shadowsOn = nameof(_shadowsOn);
        private string _token = nameof(_token);
        private string _email = nameof(_email);
        private string _vSyncCount = nameof(_vSyncCount);
        
        public event Action<bool> OnShadowsChanged;
        public event Action<string> OnEmailChanged;

        public int VSyncCount
        {
            get => GetIntValue(_vSyncCount);

            set => SetIntValue(_vSyncCount, value);
        }

        public bool ShadowsOn
        {
            get => GetBoolValue(_shadowsOn);

            set
            {
                SetBoolValue(_shadowsOn, value);
                OnShadowsChanged?.Invoke(value);
            }
        }

        public string Token
        {
            get => GetStringValue(_token);

            set => SetStringValue(_token, value);
        }

        private int GetIntValue(string key, int defaultValue = 0)
        {
            if (PlayerPrefs.HasKey(key))
                return PlayerPrefs.GetInt(key);

            return defaultValue;
        }

        private string GetStringValue(string key, string defaultValue = "")
        {
            if (PlayerPrefs.HasKey(key))
                return PlayerPrefs.GetString(key);

            return defaultValue;
        }
        
        private bool GetBoolValue(string key, bool defaultValue = true)
        {
            if (PlayerPrefs.HasKey(key))
            {
                var intValue = PlayerPrefs.GetInt(key);
                switch (intValue)
                {
                    case 0:
                        return false;
                    case 1:
                        return true;
                    default:
                        SetBoolValue(key, defaultValue);
                        return defaultValue;
                }
            }

            return defaultValue;
        }

        private void SetIntValue(string key, int value) 
            => PlayerPrefs.SetInt(key, value);
        
        private void SetStringValue(string key, string value)
            => PlayerPrefs.SetString(key, value);
        
        private void SetBoolValue(string key, bool value)
            => PlayerPrefs.SetInt(key, value ? 1 : 0);
    }
}