using System;
using Lean.Gui;

namespace Omega.Kulibin
{
    public sealed class LeanToggleWrapper
    {
        private readonly LeanToggle _toggle;

        private bool _notifying;

        public event Action<bool> OnStateChanged;

        public LeanToggleWrapper(LeanToggle toggle)
        {
            _toggle = toggle;
            
            _toggle.OnOn.AddListener(SendOnState);
            _toggle.OnOff.AddListener(SendOffState);
            
            _notifying = true;
        }

        public void Set(bool value)
        {
            _toggle.Set(value);
        }

        public void SetWithoutNotify(bool value)
        {
            _notifying = false;
            _toggle.Set(value);
            _notifying = true;
        }

        private void SendOffState()
        {
            if (_notifying)
                OnStateChanged?.Invoke(false);
        }

        private void SendOnState()
        {
            if (_notifying)
                OnStateChanged?.Invoke(true);
        }
    }
}