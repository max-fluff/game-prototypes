using Omega.IoC;

namespace MaxFluff.Prototypes
{
    public class PlatformerState : GameState<PlatformerContext>
    {
        protected override string RequiredSceneName => AppScenes.PlatformerScene.Name;

        protected override void InitState(App app)
        {
            base.InitState(app);

            _container = _container.ConfigureScoped(c =>
            {
                c.AddSingleton(_context.PlayerView);
                c.AddSingleton(_context.MainCameraView);
                c.AddSingleton(_context.StateBasedGameObjectsController);
                c.AddSingleton(_context.ScoreCounterView);

                c.AddSingleton<PlatformerPlayerPresenter>();
                c.AddSingleton<CameraPresenter>();
                c.AddSingleton<StateBasedGameObjectsControllerPresenter>();
                c.AddSingleton<ScoreCounterPresenter>();

                c.AddSingleton<PlatformerPlayerInputBinding>();
                c.AddSingleton<PlayerCameraBinding>();
            });

            _core.Add(_container.Resolve<PlatformerPlayerInputBinding>())
                .Add(_container.Resolve<PlayerCameraBinding>());

            _core.Init();
        }
    }
}