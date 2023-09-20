using System;

namespace Omega.Kulibin
{
    [Flags]
    public enum SlotType
    {
        None = 0,
        ForwardLeft = 1 << 1,
        ForwardRight = 1 << 2,
        UpLeft = 1 << 3,
        UpRight = 1 << 4,
        Bumper = BumperBackLeft | BumperBackRight | BumperForwardLeft | BumperForwardCenter | BumperForwardRight,
        BackBumper = BackBumperBackLeft | BackBumperBackRight | BackBumperForwardLeft | BackBumperForwardCenter |
                     BackBumperForwardRight,
        Forward = ForwardLeft | ForwardRight,
        Up = UpLeft | UpRight,
        BumperBackLeft = 1 << 5,
        BumperBackRight = 1 << 6,
        BumperForwardLeft = 1 << 7,
        BumperForwardCenter = 1 << 8,
        BumperForwardRight = 1 << 9,
        BackBumperBackLeft = 1 << 10,
        BackBumperBackRight = 1 << 11,
        BackBumperForwardLeft = 1 << 12,
        BackBumperForwardCenter = 1 << 13,
        BackBumperForwardRight = 1 << 14,
        SlotForBumper = 1 << 15,
    }
}