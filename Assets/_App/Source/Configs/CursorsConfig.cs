using System;
using UnityEngine;

namespace Omega.Kulibin
{
    [CreateAssetMenu(fileName = "CursorsConfig", menuName = "Config/Cursors", order = 0)]
    public sealed class CursorsConfig : ScriptableObject
    {
        public CursorData[] Cursors;
    }
    
    [Serializable]
    public struct CursorData
    {
        public CursorType Type;
        public Texture2D Texture;
        public Vector2 Pivot;
    }
}