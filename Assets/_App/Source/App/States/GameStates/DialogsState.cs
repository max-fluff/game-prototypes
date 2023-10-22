using Omega.IoC;

namespace MaxFluff.Prototypes
{
    public class DialogsState : GameState<DialogsContext>
    {
        protected override string RequiredSceneName => AppScenes.DialogsScene.Name;

        protected override void InitState(App app)
        {
            base.InitState(app);

            _container = _container.ConfigureScoped(c =>
            {
                c.AddSingleton(_context.Dialogue);

                c.AddSingleton(_context.ReplyView);
                c.AddSingleton(_context.CurrentLineView);
                c.AddSingleton(_context.CounterView);
                c.AddSingleton(_context.PersonView);

                c.AddSingleton<ReplyPresenter>();
                c.AddSingleton<CurrentLinePresenter>();
                c.AddSingleton<CounterPresenter>();
                c.AddSingleton<PersonPresenter>();

                c.AddSingleton<GameQuitBinding>();
                c.AddSingleton<DialogBinding>();
            });

            _core.Add(_container.Resolve<GameQuitBinding>())
                .Add(_container.Resolve<DialogBinding>());

            _core.Init();
        }
    }
}