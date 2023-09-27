using Cysharp.Threading.Tasks;
using Omega.IoC;
using UnityEngine;

namespace MaxFluff.Prototypes
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
                c.AddSingleton(_context.QuitWindow);
                c.AddSingleton(_context.RaycastView);
                c.AddSingleton(_context.GamesListView);

                c.AddSingleton<StartScreenEvents>();

                c.AddSingleton<StartScreenPresenter>();
                c.AddSingleton<RaycastPresenter>();
                c.AddSingleton<GamesListPresenter>();

                c.AddSingleton<WindowsOrganizerPresenter>();
                c.AddSingleton<LoadingWindowPresenter>();
                c.AddSingleton<QuitWindowPresenter>();

                c.AddSingleton<WindowsInputBinding>();
                c.AddSingleton<EnterGameBinding>();
                c.AddSingleton<StartScreenWindowsBinding>();
                c.AddSingleton<StartScreenQuitBinding>();
            });

            _core = new AppCore();
            _core.OnStateChangeRequested += RequestStateChange;
            _core.Add(_container.Resolve<WindowsInputBinding>())
                .Add(_container.Resolve<EnterGameBinding>())
                .Add(_container.Resolve<StartScreenWindowsBinding>())
                .Add(_container.Resolve<StartScreenQuitBinding>());
            _core.Init();
        }
    }
}