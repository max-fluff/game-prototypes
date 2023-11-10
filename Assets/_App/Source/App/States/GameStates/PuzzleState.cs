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
                c.AddSingleton(_context.SheetView);
                c.AddSingleton(_context.SendButton);
                c.AddSingleton(_context.ContactsList);
                c.AddSingleton(_context.DataSheets);
                c.AddSingleton(_context.Phone);

                c.AddSingleton<CameraPresenter>();
                c.AddSingleton<SheetPresenter>();
                c.AddSingleton<SendButtonPresenter>();
                c.AddSingleton<PhonePresenter>();

                c.AddSingleton<GameQuitBinding>();
                c.AddSingleton<PuzzleSheetBinding>();
                c.AddSingleton<PuzzlePhoneBinding>();
            });

            _core.Add(_container.Resolve<GameQuitBinding>())
                .Add(_container.Resolve<PuzzleSheetBinding>())
                .Add(_container.Resolve<PuzzlePhoneBinding>());

            _core.Init();
        }
    }
}