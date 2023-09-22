using System;
using System.Collections.Generic;
using UnityEngine;

namespace MaxFluff.Prototypes
{
    [CreateAssetMenu(fileName = "GamesList", menuName = "Config/Games", order = 0)]
    public class GamesList : ScriptableObject
    {
        public List<GameData> Games;
    }

    [Serializable]
    public struct GameData
    {
        public string Name;
        [SerializeReference] public IAppState State;
    }
}