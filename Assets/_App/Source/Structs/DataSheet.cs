using System;
using System.Collections.Generic;

namespace MaxFluff.Prototypes
{
    [Serializable]
    public struct DataSheet
    {
        public string Name;
        public Gender Gender;
        public int DayOfBirth;
        public int MonthOfBirth;
        public int YearOfBirth;
        public List<string> ChildrenNames;
        public SpouseData Spouse;
        public string TaxNumber;
        public string PhoneNumber;
        public int AddressBld;
        public string AddressStreet;
        public string AddressCity;
        public PersonData FilledData;
    }

    [Serializable]
    public class SpouseData
    {
        public string Name;
        public string PhoneNumber;
    }

    public enum Gender
    {
        None,
        Other,
        M,
        F
    }
}