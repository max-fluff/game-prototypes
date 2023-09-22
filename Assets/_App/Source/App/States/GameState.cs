using Cysharp.Threading.Tasks;
using Omega.IoC;
using Object = UnityEngine.Object;

namespace MaxFluff.Prototypes
{
    public abstract class GameState<T> : AppStateBase<T> where T : GameContext
    {
        protected SceneChangerService SceneChanger;
        protected AppScenes AppScenes;

        protected abstract string RequiredSceneName { get; }

        protected override async UniTask InitContext(App app)
        {
            AppScenes = app.Services.Resolve<AppScenes>();
            while ((_context = Object.FindObjectOfType<T>()) == null)
            {
                await app.Services.Resolve<SceneChangerService>().SwitchToScene(RequiredSceneName);
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
                .Add(_container.Resolve<GameWindowsBinding>());
        }

        public override async UniTask Destroy(App app)
        {
            _core.Destroy();
            await SceneChanger.UnloadSceneAsync(RequiredSceneName);
        }
    }
}