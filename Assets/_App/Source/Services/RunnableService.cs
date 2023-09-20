using System.Threading;
using Cysharp.Threading.Tasks;

namespace MaxFluff.Prototypes
{
    public abstract class RunnableService
    {
        private readonly CancellationTokenSource _cancelToken;

        protected RunnableService()
        {
            _cancelToken = new CancellationTokenSource();
            Run(_cancelToken.Token).ToCancellationToken();
        }

        protected abstract UniTask Run(CancellationToken cancellationToken);

        public void StopService() => _cancelToken.Cancel();

        public void ContinueService()
        {
            if (_cancelToken.IsCancellationRequested)
            {
                _cancelToken.Dispose();
                Run(_cancelToken.Token);
            }
        }
    }
}