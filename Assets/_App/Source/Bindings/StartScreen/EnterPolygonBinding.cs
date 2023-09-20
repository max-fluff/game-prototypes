using System;

namespace MaxFluff.Prototypes
{
    public sealed class EnterPolygonBinding : IInitBinding, IStateChangerBinding
    {
        private readonly LoadingWindowPresenter _loadingPresenter;
        private readonly StartScreenEvents _events;

        public event Action<IAppState> OnStateChangeRequested;

        public EnterPolygonBinding(
            LoadingWindowPresenter loadingPresenter,
            StartScreenEvents events
        )
        {
            _loadingPresenter = loadingPresenter;
            _events = events;
        }

        public void Init()
        {
            _events.OnOpenPolygon += SwitchToWorkspace;
        }

        private void SwitchToWorkspace()
        {
            _loadingPresenter.Open();
            OnStateChangeRequested?.Invoke(new WorkspaceState());
        }
    }
}