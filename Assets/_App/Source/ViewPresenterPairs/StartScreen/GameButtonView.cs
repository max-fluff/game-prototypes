using System;
using Lean.Gui;
using TMPro;

namespace MaxFluff.Prototypes
{
    public class GameButtonView : ViewBase
    {
        public LeanButton Button;
        public TextMeshProUGUI Name;
    }

    public class GameButtonPresenter : PresenterBase<GameButtonView>
    {
        private readonly GameData _gameData;
        public event Action<IAppState> OnGameLaunchRequested;

        public GameButtonPresenter(GameButtonView view, GameData gameData) : base(view)
        {
            _gameData = gameData;
            view.Button.OnClick.AddListener(SendOnGameLaunchRequested);
            view.Name.SetText(gameData.Name);

            if (gameData.State == null)
                view.Button.interactable = false;
        }

        private void SendOnGameLaunchRequested() => 
            OnGameLaunchRequested?.Invoke(_gameData.State);
    }
}