using System;
using Cysharp.Threading.Tasks;

namespace Omega.Kulibin
{
    public sealed class EmptyState : IAppState
    {
        public event Func<IAsyncState<App>, UniTask> OnStateChangeRequested;
        public async UniTask Init(App app) { }
        public async UniTask Run(App app) { }
        public async UniTask Destroy(App app) { }
    }
}