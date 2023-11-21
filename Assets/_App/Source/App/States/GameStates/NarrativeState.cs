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

                
            });

            _core.Init();
        }
    }
}