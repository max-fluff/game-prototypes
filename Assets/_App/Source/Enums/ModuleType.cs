using System;

namespace Omega.Kulibin
{
    [Flags]
    public enum ModuleType
    {
        None = 0,
        All = ~None,
        Single = LED | Sound | Touch | Button | Line | Illumination | Color,
        Double = Rangefinder | Magnet,
        AdditionalPort = Rangefinder | Magnet,
        Indicator = LED | Sound | Magnet,
        Sensor = Touch | Button | Line | Illumination | Color,
        LED = 1,
        Sound = 1 << 1,
        Touch = 1 << 2,
        Button = 1 << 3,
        Line = 1 << 4,
        Illumination = 1 << 5,
        Rangefinder = 1 << 6,
        Bumper = 1 << 7,
        Color = 1 << 8,
        Magnet = 1 << 9,
    }
}