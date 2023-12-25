using MaxFluff.Prototypes._App.Source.Bindings;
using Omega.IoC;

namespace MaxFluff.Prototypes
{
    public class TBSState : GameState<TBSContext>
    {
        protected override string RequiredSceneName => AppScenes.TBSScene.Name;

        protected override void InitState(App app)
        {
            base.InitState(app);

            _container = _container.ConfigureScoped(c =>
            {
                c.AddSingleton(_context.BoardView);

                c.AddSingleton<BoardPresenter>();

                c.AddSingleton<GameQuitBinding>();
                c.AddSingleton<TBSBinding>();
            });
            _core
                .Add(_container.Resolve<GameQuitBinding>())
                .Add(_container.Resolve<TBSBinding>());


            _core.Init();
        }
    }
}