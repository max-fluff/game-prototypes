using Cysharp.Threading.Tasks;
using MaxFluff.Prototypes.Games;
using Omega.IoC;

namespace MaxFluff.Prototypes
{
    public class PlatformerState : GameState<PlatformerContext>
    {
        protected override string RequiredSceneName => AppScenes.PlatformerScene.Name;

        protected override void InitState(App app)
        {
            base.InitState(app);
            /*
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

            _core.Add(_container.Resolve<WindowsInputBinding>())
                .Add(_container.Resolve<SettingsBinding>())
                .Add(_container.Resolve<GameWindowsBinding>());
            */
            _core.Init();
        }
    }
}