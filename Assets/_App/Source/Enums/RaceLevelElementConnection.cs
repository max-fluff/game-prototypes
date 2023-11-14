using System;

namespace MaxFluff.Prototypes
{
    [Flags]
    public enum RaceLevelElementConnection
    {
        None = 0,
        Up = 1,
        Down = 1 << 1,
        Left = 1 << 2,
        Right = 1 << 3,
        CenterVertical = 1 << 4,
        CenterHorizontal = 1 << 5
    }
}