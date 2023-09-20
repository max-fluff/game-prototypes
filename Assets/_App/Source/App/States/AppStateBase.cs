using System;
using Cysharp.Threading.Tasks;
using Omega.IoC;

namespace MaxFluff.Prototypes
{
    public abstract class AppStateBase<TContext> : IAppState where TContext : SceneContextBase
    {
        protected IoContainer _container;
        protected AppCore _core;
        protected TContext _context;
        public event Func<IAsyncState<App>, UniTask> OnStateChangeRequested;


        public async UniTask Init(App app)
        {
            await InitContext(app);
            InitState(app);
        }
        
        protected abstract UniTask InitContext(App app);

        protected abstract void InitState(App app);

        public virtual async UniTask Run(App app) =>
            _core.Run();

        public virtual async UniTask Destroy(App app) => 
            _core.Destroy();

        protected void RequestStateChange(IAppState state) => 
            OnStateChangeRequested?.Invoke(state);
    }
}