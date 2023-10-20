using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace MaxFluff.Prototypes
{
    [CreateAssetMenu(fileName = FILE_NAME, menuName = "Config/Dialogue", order = 0)]
    public class Dialogue : ScriptableObject
    {
        private const string FILE_NAME = "Dialogue";

        public List<Line> Lines = new List<Line>();
    }

    [Serializable]
    public struct Line
    {
        public int defaultPoints;
        public string defaultLine;

        public List<LineVariant> variants;
    }

    [Serializable]
    public class LineVariant
    {
        [MinMaxSlider(0, 1, true)] 
        public Vector2 passionRange = Vector2.up;
        
        [MinMaxSlider(0, 1, true)] 
        public Vector2 confidenceRange = Vector2.up;
        
        [MinMaxSlider(0, 1, true)] 
        public Vector2 smartRange = Vector2.up;
        
        [MinMaxSlider(0, 1, true)] 
        public Vector2 positivityRange = Vector2.up;

        public int points;
        public string line;
    }
}