namespace Omega.Kulibin
{
    public sealed class PlayerPrefsBinding : IInitBinding, IDestroyBinding
    {
        private readonly LightPresenter _lightPresenter;
        private readonly PlayerPrefsService _playerPrefsService;

        public PlayerPrefsBinding(
            LightPresenter lightPresenter,
            PlayerPrefsService playerPrefsService)
        {
            _lightPresenter = lightPresenter;
            _playerPrefsService = playerPrefsService;
        }

        public void Init()
        {
            _lightPresenter.SetShadowsMode(_playerPrefsService.ShadowsOn);
            _playerPrefsService.OnShadowsChanged += _lightPresenter.SetShadowsMode;
        }

        public void Destroy()
        {
            _playerPrefsService.OnShadowsChanged -= _lightPresenter.SetShadowsMode;
        }
    }
}