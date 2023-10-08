using System;

namespace MaxFluff.Prototypes
{
    [Flags]
    public enum PersonData
    {
        None = 0,
        Name = 1,
        Gender = 1 << 1,
        Day = 1 << 2,
        Month = 1 << 3,
        Year = 1 << 4,
        BirthDay = Day | Month | Year,
        Children = 1 << 5,
        Spouse = 1 << 6,
        TaxNum = 1 << 7,
        PhoneNum = 1 << 8,
        AddressCity = 1 << 9,
        AddressStreet = 1 << 10,
        AddressBld = 1 << 11,
        Address = AddressCity | AddressStreet | AddressBld
    }
}