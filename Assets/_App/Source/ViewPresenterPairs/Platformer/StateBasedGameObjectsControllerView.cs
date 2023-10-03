using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MaxFluff.Prototypes
{
    public class StateBasedGameObjectsControllerView : ViewBase
    {
    }

    public class StateBasedGameObjectsControllerPresenter : PresenterBase<StateBasedGameObjectsControllerView>
    {
        private List<PlayerStateBasedGameObject> _playerStateBasedGameObjects;

        public StateBasedGameObjectsControllerPresenter(StateBasedGameObjectsControllerView view) : base(view)
        {
            _playerStateBasedGameObjects = Object.FindObjectsOfType<PlayerStateBasedGameObject>().ToList();
        }

        public void SetState(PlatformerPlayerState state)
        {
            foreach (var playerStateBasedGameObject in _playerStateBasedGameObjects)
            {
                playerStateBasedGameObject.gameObject.SetActive(playerStateBasedGameObject.ActiveAtState ^
                                                                playerStateBasedGameObject.State != state);
            }
        }
    }
}