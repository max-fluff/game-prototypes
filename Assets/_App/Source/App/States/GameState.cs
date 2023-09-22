using Cysharp.Threading.Tasks;
using Omega.IoC;
using Object = UnityEngine.Object;

namespace MaxFluff.Prototypes
{
    public sealed class GameState : AppStateBase<GameContext>
    {
        private SceneChangerService _sceneChanger;
        private AppScenes _appScenes;

        protected override async UniTask InitContext(App app)
        {
            while ((_context = Object.FindObjectOfType<GameContext>()) == null)
            {
                var requiredScene = app.Services.Resolve<AppScenes>().Game.Name;
                await app.Services.Resolve<SceneChangerService>().SwitchToScene(requiredScene);
            }
        }

        protected override void InitState(App app)
        {
            _container = app.Services.ConfigureScoped(c =>
            {
                c.AddSingleton(_context.UICamera);
                c.AddSingleton(_context.RaycastView);

                c.AddSingleton(_context.UI.WindowsOrganizer);
                c.AddSingleton(_context.UI.LoadingWindow);

                c.AddSingleton<GameEvents>();

                c.AddSingleton<WindowsOrganizerPresenter>();
                c.AddSingleton<LoadingWindowPresenter>();

                c.AddSingleton<ICameraPresenter, CameraPresenter>("UICamera");
                c.AddSingleton<RaycastPresenter>();

                c.AddSingleton<WindowsInputBinding>();
                c.AddSingleton<GameWindowsBinding>();
            });

            _core = new AppCore();
            _core.OnStateChangeRequested += RequestStateChange;

            _core.Add(_container.Resolve<WindowsInputBinding>())
                .Add(_container.Resolve<SettingsBinding>())
                .Add(_container.Resolve<GameWindowsBinding>());

            _core.Init();
        }

        public override async UniTask Destroy(App app)
        {
            _core.Destroy();
            await _sceneChanger.UnloadSceneAsync(_appScenes.Game.Name);
        }
    }
}