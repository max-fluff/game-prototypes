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
                
                c.AddSingleton<FPSPlayerPresenter>();
                c.AddSingleton<CameraPresenter>();
                
                c.AddSingleton<GameQuitBinding>();
                c.AddSingleton<FPSPlayerInputBinding>();
            });

            _core.Add(_container.Resolve<GameQuitBinding>())
                .Add(_container.Resolve<FPSPlayerInputBinding>());

            _core.Init();
        }
    }
}