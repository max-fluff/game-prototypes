using Omega.IoC;

namespace MaxFluff.Prototypes
{
    public class NarrativeState : GameState<NarrativeContext>
    {
        protected override string RequiredSceneName => AppScenes.NarrativeScene.Name;

        protected override void InitState(App app)
        {
            base.InitState(app);

            _container = _container.ConfigureScoped(c =>
            {
                c.AddSingleton(_context.PenView);
                c.AddSingleton(_context.StampView);
                c.AddSingleton(_context.MainCameraView);
                c.AddSingleton(_context.SheetStackView);

                c.AddSingleton<PenPresenter>();
                c.AddSingleton<StampPresenter>();
                c.AddSingleton<CameraPresenter>();
                c.AddSingleton<SheetStackPresenter>();

                c.AddSingleton<NarrativeBinding>();
                c.AddSingleton<GameQuitBinding>();
            });
            _core.Add(_container.Resolve<NarrativeBinding>())
                .Add(_container.Resolve<GameQuitBinding>());


            _core.Init();
        }
    }
}