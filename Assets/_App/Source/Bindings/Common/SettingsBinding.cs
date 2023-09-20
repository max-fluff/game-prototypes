namespace MaxFluff.Prototypes
{
    public sealed class SettingsBinding : IInitBinding, IDestroyBinding
    {
        private readonly SettingsWindowPresenter _settingsWindow;
        private readonly QualitySettingsService _qualitySettingsService;
        private readonly PlayerPrefsService _playerPrefsService;
        
        private ButtonWindowPair _buttonWindowPair;

        public SettingsBinding(
            SettingsWindowPresenter settingsWindow,
            QualitySettingsService qualitySettingsService,
            PlayerPrefsService playerPrefsService)
        {
            _settingsWindow = settingsWindow;
            _qualitySettingsService = qualitySettingsService;
            _playerPrefsService = playerPrefsService;
        }

        public void Init()
        {
            _settingsWindow.OnQualityPresetChanged += QualityPresetChanged;
            _settingsWindow.OnVSyncCountChanged += OnVSyncCountChanged;
            _settingsWindow.OnShadowsModeChanged += OnShadowsModeChanged;
            _settingsWindow.OnOpened += UpdateSettingsWindowState;

            _settingsWindow.CachedShadowsToggleState = _playerPrefsService.ShadowsOn;
        }

        private void OnShadowsModeChanged(bool value)
        {
            _playerPrefsService.ShadowsOn = value;
        }

        private void OnVSyncCountChanged(int value)
        {
            _qualitySettingsService.UpdateVSyncCount(value);
            _playerPrefsService.VSyncCount = value;
        }

        private void QualityPresetChanged(int qualityPresetIndex)
        {
            _qualitySettingsService.UpdateQualitySettings(qualityPresetIndex);
        }

        private void UpdateSettingsWindowState()
        {
            var qualityPreset = _qualitySettingsService.GetQualityPresetIndex();
            var vSync = _qualitySettingsService.GetVSyncCount();
            var shadowsOn = _playerPrefsService.ShadowsOn;

            _settingsWindow.SetState(qualityPreset, vSync, shadowsOn);
        }

        public void Destroy()
        {
            _buttonWindowPair.Dispose();
        }
    }
}