using System;
using Lean.Gui;
using UnityEngine;

namespace MaxFluff.Prototypes
{
    public sealed class SettingsWindowView : WindowViewBase
    {
        public LeanToggle LowSettingsToggle;
        public LeanToggle MediumSettingsToggle;
        public LeanToggle HighSettingsToggle;
        public LeanToggle ShadowsToggle;
        public LeanToggle VSyncToggle;
    }

    public sealed class SettingsWindowPresenter : WindowPresenterBase<SettingsWindowView>
    {
        public event Action<int> OnQualityPresetChanged;
        public event Action<int> OnVSyncCountChanged;
        public event Action<bool> OnShadowsModeChanged;

        public bool CachedShadowsToggleState;
        private readonly LeanButton _shadowsButton;

        public SettingsWindowPresenter(SettingsWindowView view) : base(view)
        {
            _shadowsButton = _view.ShadowsToggle.GetComponent<LeanButton>();

            view.HighSettingsToggle.GetComponent<LeanButton>().OnClick.AddListener(() =>
            {
                view.HighSettingsToggle.TurnOn();
                SetShadowsToggleAvailability(true);
                OnQualityPresetChanged?.Invoke(2);
            });
            view.MediumSettingsToggle.GetComponent<LeanButton>().OnClick.AddListener(() =>
            {
                view.MediumSettingsToggle.TurnOn();
                SetShadowsToggleAvailability(true);
                OnQualityPresetChanged?.Invoke(1);
            });
            view.LowSettingsToggle.GetComponent<LeanButton>().OnClick.AddListener(() =>
            {
                view.LowSettingsToggle.TurnOn();
                SetShadowsToggleAvailability(false);
                OnQualityPresetChanged?.Invoke(0);
            });

            _shadowsButton.OnClick.AddListener(() =>
            {
                view.ShadowsToggle.Toggle();
                OnShadowsModeChanged?.Invoke(view.ShadowsToggle.On);
            });

            view.VSyncToggle.GetComponent<LeanButton>().OnClick.AddListener(() =>
            {
                view.VSyncToggle.Toggle();
                var value = view.VSyncToggle.On ? 1 : 0;
                OnVSyncCountChanged?.Invoke(value);
            });
        }

        public override bool NeedBlocker => false;
        public override bool ClosesOnOutsideClick => true;

        public void SetState(
            int qualityPresetIndex,
            int vSync,
            bool shadowsOn)
        {
            switch (qualityPresetIndex)
            {
                case 0:
                    _view.LowSettingsToggle.TurnOn();
                    break;
                case 1:
                    _view.MediumSettingsToggle.TurnOn();
                    break;
                case 2:
                    _view.HighSettingsToggle.TurnOn();
                    break;
            }

            switch (vSync)
            {
                case 0:
                    _view.VSyncToggle.TurnOff();
                    break;
                case 1:
                    _view.VSyncToggle.TurnOn();
                    break;
            }

            _view.ShadowsToggle.On = shadowsOn;
        }

        private void SetShadowsToggleAvailability(bool isAvailable)
        {
            if (isAvailable == _shadowsButton.interactable) return;

            if (!isAvailable)
                CachedShadowsToggleState = _view.ShadowsToggle.On;

            var isOn = isAvailable && CachedShadowsToggleState;
            _view.ShadowsToggle.On = isOn;
            OnShadowsModeChanged?.Invoke(isOn);

            _shadowsButton.interactable = isAvailable;
        }

        public void SetPositionX(float x) => _view.transform.position =
            new Vector3(x, _view.transform.position.y, _view.transform.position.z);
    }
}