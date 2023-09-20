using System;

namespace Omega.Kulibin
{
    public interface IStateChangerBinding : IBinding
    {
        public event Action<IAppState> OnStateChangeRequested;
    }
}