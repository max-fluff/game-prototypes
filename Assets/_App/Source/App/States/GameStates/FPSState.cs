using MaxFluff.Prototypes.FPS;
using Omega.IoC;

namespace MaxFluff.Prototypes
{
    public class FPSState : GameState<FPSContext>
    {
        protected override string RequiredSceneName => AppScenes.FPSScene.Name;

        protected override void InitState(App app)
        {
            base.InitState(app);

            _container = _container.ConfigureScoped(c =>
            {
                c.AddSingleton(_context.PlayerView);
                c.AddSingleton(_context.MainCamera);
                c.AddSingleton(_context.Core);
                c.AddSingleton(_context.HealthVisualization);
                c.AddSingleton(_context.FailWindow);
                c.AddSingleton(_context.WinWindow);
                c.AddSingleton(_context.CoreVisualView);
                c.AddSingleton(_context.MobileRetranslatorPowerUpView);
                c.AddSingleton(_context.MobileRetranslatorVisualizerView);

                c.AddSingleton<FPSPlayerPresenter>();
                c.AddSingleton<CameraPresenter>();
                c.AddSingleton<CorePresenter>();
                c.AddSingleton<HealthVisualizationPresenter>();
                c.AddSingleton<FailWindowPresenter>();
                c.AddSingleton<WinWindowPresenter>();
                c.AddSingleton<CoreVisualPresenter>();
                c.AddSingleton<MobileRetranslatorPowerUpPresenter>();
                c.AddSingleton<MobileRetranslatorVisualizerPresenter>();

                c.AddSingleton<GameQuitBinding>();
                c.AddSingleton<FPSPlayerInputBinding>();
                c.AddSingleton<CoreBinding>();
                c.AddSingleton<HealthBinding>();
                c.AddSingleton<FPSWindowsBinding>();
            });

            _core.Add(_container.Resolve<GameQuitBinding>())
                .Add(_container.Resolve<FPSPlayerInputBinding>())
                .Add(_container.Resolve<CoreBinding>())
                .Add(_container.Resolve<HealthBinding>())
                .Add(_container.Resolve<FPSWindowsBinding>());

            _core.Init();
        }
    }
}