using System;

namespace MaxFluff.Prototypes
{
    public sealed class EnterGameBinding : IInitBinding, IStateChangerBinding
    {
        private readonly LoadingWindowPresenter _loadingPresenter;
        private readonly GamesListPresenter _gamesListPresenter;
        private readonly GamesList _gamesList;
        public event Action<IAppState> OnStateChangeRequested;

        public EnterGameBinding(
            LoadingWindowPresenter loadingPresenter,
            GamesListPresenter gamesListPresenter,
            GamesList gamesList
        )
        {
            _loadingPresenter = loadingPresenter;
            _gamesListPresenter = gamesListPresenter;
            _gamesList = gamesList;
        }

        public void Init()
        {
            _gamesListPresenter.Init(_gamesList.Games);
            _gamesListPresenter.OnStateChangeRequested += SendOnStateChangeRequested;
        }

        private void SendOnStateChangeRequested(IAppState state)
        {
            _loadingPresenter.Open();
            OnStateChangeRequested?.Invoke(state);
        }
    }
}