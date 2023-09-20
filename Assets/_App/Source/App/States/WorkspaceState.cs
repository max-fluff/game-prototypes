using Cysharp.Threading.Tasks;
using Omega.IoC;
using Object = UnityEngine.Object;

namespace Omega.Kulibin
{
    public sealed class WorkspaceState : AppStateBase<WorkspaceContext>
    {
        private SceneChangerService _sceneChanger;
        private AppScenes _appScenes;
        protected override async UniTask InitContext(App app)
        {
            _sceneChanger = app.Services.Resolve<SceneChangerService>();
            _appScenes = app.Services.Resolve<AppScenes>();
            var requiredScene = _appScenes.Workspace.Name;

            _context = Object.FindObjectOfType<WorkspaceContext>();

            while (_context == null)
            {
                var workspaceLoadTask = _context != null
                    ? _sceneChanger.ReloadScene(requiredScene)
                    : _sceneChanger.AddSceneAsync(requiredScene);

                await workspaceLoadTask;

                _context = Object.FindObjectOfType<WorkspaceContext>();
            }
        }

        protected override void InitState(App app)
        {
            _container = app.Services.ConfigureScoped(c =>
            {
                c.AddSingleton(_context.UICamera);
                c.AddSingleton(_context.RaycastView);

                c.AddSingleton(_context.UI.WindowsOrganizer);
                c.AddSingleton(_context.UI.SettingsWindow);
                c.AddSingleton(_context.UI.LoadingWindow);
                
                c.AddSingleton<WorkspaceEvents>();

                c.AddSingleton<FpsCounterPresenter>();

                c.AddSingleton<WindowsOrganizerPresenter>();
                c.AddSingleton<SettingsWindowPresenter>();
                c.AddSingleton<LoadingWindowPresenter>();

                c.AddSingleton<ICameraPresenter, CameraPresenter>("UICamera");
                c.AddSingleton<LightPresenter>();
                c.AddSingleton<RaycastPresenter>();

                c.AddSingleton<FPSBinding>();
                c.AddSingleton<WindowsInputBinding>();
                c.AddSingleton<WorkspaceWindowsBinding>();
                c.AddSingleton<SettingsBinding>();
                c.AddSingleton<PlayerPrefsBinding>();
            });

            _core = new AppCore();
            _core.OnStateChangeRequested += RequestStateChange;

            _core.Add(_container.Resolve<WindowsInputBinding>())
                .Add(_container.Resolve<FPSBinding>())
                .Add(_container.Resolve<SettingsBinding>())
                .Add(_container.Resolve<PlayerPrefsBinding>())
                .Add(_container.Resolve<WorkspaceWindowsBinding>());

            _core.Init();
        }

        public override async UniTask Run(App app) =>
            _core.Run();

        public override async UniTask Destroy(App app)
        {
            _core.Destroy();
            await _sceneChanger.UnloadSceneAsync(_appScenes.Workspace.Name);
        }
    }
}