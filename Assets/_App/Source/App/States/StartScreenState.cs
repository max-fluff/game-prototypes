using Cysharp.Threading.Tasks;
using Omega.IoC;
using UnityEngine;

namespace Omega.Kulibin
{
    public sealed class StartScreenState : AppStateBase<StartScreenContext>
    {
        protected override async UniTask InitContext(App app)
        {
            while ((_context = Object.FindObjectOfType<StartScreenContext>()) == null)
            {
                var requiredScene = app.Services.Resolve<AppScenes>().StartScreen.Name;
                await app.Services.Resolve<SceneChangerService>().SwitchToScene(requiredScene);
            }
        }

        protected override void InitState(App app)
        {
            _container = app.Services.ConfigureScoped(c =>
            {
                c.AddSingleton(_context.StartScreen);

                c.AddSingleton(_context.WindowsOrganizer);
                c.AddSingleton(_context.LoadingWindow);
                c.AddSingleton(_context.RaycastView);
                
                c.AddSingleton<StartScreenEvents>();

                c.AddSingleton<StartScreenPresenter>();
                c.AddSingleton<RaycastPresenter>();
                
                c.AddSingleton<WindowsOrganizerPresenter>();
                c.AddSingleton<LoadingWindowPresenter>();

                c.AddSingleton<WindowsInputBinding>();
                c.AddSingleton<EnterPolygonBinding>();
                c.AddSingleton<StartScreenLanguageBinding>();
                c.AddSingleton<StartScreenWindowsBinding>();
            });
            
            _core = new AppCore();
            _core.OnStateChangeRequested += RequestStateChange;
            _core.Add(_container.Resolve<WindowsInputBinding>())
                .Add(_container.Resolve<EnterPolygonBinding>())
                .Add(_container.Resolve<StartScreenLanguageBinding>())
                .Add(_container.Resolve<StartScreenWindowsBinding>());
            _core.Init();
        }

        public override async UniTask Run(App app)
        {
            _core.Run();
        }
    }
}