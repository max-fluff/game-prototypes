using System;

namespace MaxFluff.Prototypes
{
    public interface IStateChangerBinding : IBinding
    {
        public event Action<IAppState> OnStateChangeRequested;
    }
}