using Omega.IoC;

namespace MaxFluff.Prototypes
{
    public class TycoonState : GameState<TycoonContext>
    {
        protected override string RequiredSceneName => AppScenes.TycoonState.Name;

        protected override void InitState(App app)
        {
            base.InitState(app);

            _container = _container.ConfigureScoped(c =>
            {
                c.AddSingleton(_context.MainCameraView);
                c.AddSingleton(_context.MoneyCounterView);
                c.AddSingleton(_context.PopularityCounterView);
                c.AddSingleton(_context.TycoonBarWindowView);
                c.AddSingleton(_context.TycoonGamePlaceWindowView);
                c.AddSingleton(_context.TycoonResultsWindowView);
                c.AddSingleton(_context.TycoonSellWindowView);
                c.AddSingleton(_context.FailWindowView);
                c.AddSingleton(_context.ContinueButtonView);

                c.AddSingleton<CameraPresenter>();
                c.AddSingleton<MoneyCounterPresenter>();
                c.AddSingleton<PopularityCounterPresenter>();
                c.AddSingleton<TycoonBarWindowPresenter>();
                c.AddSingleton<TycoonGamePlaceWindowPresenter>();
                c.AddSingleton<TycoonResultsWindowPresenter>();
                c.AddSingleton<TycoonSellWindowPresenter>();
                c.AddSingleton<FailWindowPresenter>();
                c.AddSingleton<ContinueButtonPresenter>();

                c.AddSingleton<GameQuitBinding>();
                c.AddSingleton<TycoonBinding>();
                c.AddSingleton<TycoonWindowsBinding>();
            });

            _core.Add(_container.Resolve<GameQuitBinding>())
                .Add(_container.Resolve<TycoonBinding>())
                .Add(_container.Resolve<TycoonWindowsBinding>());

            _core.Init();
        }
    }
}