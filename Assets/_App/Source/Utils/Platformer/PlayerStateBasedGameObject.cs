using UnityEngine;

namespace MaxFluff.Prototypes
{
    public class PlayerStateBasedGameObject : MonoBehaviour
    {
        public PlatformerPlayerState State;
        public bool ActiveAtState = true;
    }
}