using System;
using Cysharp.Threading.Tasks;

namespace Omega.Kulibin
{
    public interface IAsyncState<in TSubject> where TSubject : IStateSubject
    {
        public event Func<IAsyncState<TSubject>, UniTask> OnStateChangeRequested;
        public UniTask Init(TSubject subject);
        public UniTask Run(TSubject subject);
        public UniTask Destroy(TSubject subject);
    }
}