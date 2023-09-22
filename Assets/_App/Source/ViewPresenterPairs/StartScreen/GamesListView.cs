using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Object = UnityEngine.Object;

namespace MaxFluff.Prototypes
{
    public class GamesListView : ViewBase
    {
        public GameButtonView GameButtonPrefab;
        public Transform Container;
    }

    public class GamesListPresenter : PresenterBase<GamesListView>
    {
        private readonly List<GameButtonPresenter> _gameButtons = new List<GameButtonPresenter>();

        public event Action<IAppState> OnStateChangeRequested;

        public GamesListPresenter(GamesListView view) : base(view)
        {
        }

        public void Init(IEnumerable<GameData> gamesList)
        {
            foreach (var gameButtonPresenter in from gameData in gamesList
                     let gameButtonView = Object.Instantiate(_view.GameButtonPrefab, _view.Container)
                     select new GameButtonPresenter(gameButtonView, gameData))
            {
                _gameButtons.Add(gameButtonPresenter);
                gameButtonPresenter.OnGameLaunchRequested += RequestStateChange;
            }
        }

        private void RequestStateChange(IAppState state) => OnStateChangeRequested?.Invoke(state);
    }
}