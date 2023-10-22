using Omega.IoC;

namespace MaxFluff.Prototypes
{
    public class RaceState : GameState<RaceContext>
    {
        protected override string RequiredSceneName => AppScenes.RaceScene.Name;

        protected override void InitState(App app)
        {
            base.InitState(app);

            _container = _container.ConfigureScoped(c =>
            {
                c.AddSingleton(_context.MainCameraView);
                c.AddSingleton(_context.Player);
                c.AddSingleton(_context.GravityChangeVisualizer);
                c.AddSingleton(_context.TimerVisualizerView);
                c.AddSingleton(_context.FailWindowView);
                c.AddSingleton(_context.TimeResultWindowView);
                c.AddSingleton(_context.BordersView);
                c.AddSingleton(_context.FinishView);

                c.AddSingleton<CameraPresenter>();
                c.AddSingleton<RacePlayerPresenter>();
                c.AddSingleton<GravityChangeVisualizerPresenter>();
                c.AddSingleton<TimerVisualizerPresenter>();
                c.AddSingleton<FailWindowPresenter>();
                c.AddSingleton<TimeResultWindowPresenter>();
                c.AddSingleton<BordersPresenter>();
                c.AddSingleton<FinishPresenter>();

                c.AddSingleton<GameQuitBinding>();
                c.AddSingleton<RacePlayerCameraBinding>();
                c.AddSingleton<RacePlayerInputBinding>();
                c.AddSingleton<RaceTimerAndResetBinding>();
                c.AddSingleton<RaceWindowsBinding>();
            });

            _core.Add(_container.Resolve<GameQuitBinding>())
                .Add(_container.Resolve<RacePlayerCameraBinding>())
                .Add(_container.Resolve<RacePlayerInputBinding>())
                .Add(_container.Resolve<RaceTimerAndResetBinding>())
                .Add(_container.Resolve<RaceWindowsBinding>());

            _core.Init();
        }
    }
}