using System.Collections.Generic;
using UnityEngine;

namespace MaxFluff.Prototypes
{
    [CreateAssetMenu(fileName = "PuzzleDataSheets", menuName = "Config/Puzzle/DataSheets", order = 0)]
    public class DataSheetsForPuzzle : ScriptableObject
    {
        public List<DataSheet> DataSheets;
    }
}