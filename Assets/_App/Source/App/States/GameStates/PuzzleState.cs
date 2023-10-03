using Omega.IoC;

namespace MaxFluff.Prototypes
{
    public class PuzzleState : GameState<PuzzleContext>
    {
        protected override string RequiredSceneName => AppScenes.PuzzleScene.Name;

        protected override void InitState(App app)
        {
            base.InitState(app);

            _container = _container.ConfigureScoped(c =>
            {
                c.AddSingleton(_context.MainCameraView);
                
                c.AddSingleton<CameraPresenter>();

                c.AddSingleton<PlatformerQuitBinding>();
            });

            _core.Add(_container.Resolve<PlatformerQuitBinding>());

            _core.Init();
        }
    }
}