using Cysharp.Threading.Tasks;
using Lean.Transition;
using TMPro;
using UnityEngine;

namespace MaxFluff.Prototypes
{
    public class SheetView : ViewBase
    {
        public TMP_InputField Name;
        public TMP_Dropdown Gender;
        public TMP_InputField BDay;
        public TMP_Dropdown BMonth;
        public TMP_InputField BYear;
        public TMP_InputField PhoneNum;
        public TMP_InputField TaxNum;
        public TMP_Dropdown City;
        public TMP_Dropdown Street;
        public TMP_InputField Bld;
        public TMP_InputField PartnerName;
        public TMP_InputField PartnerPhone;
        public TMP_InputField Child1;
        public TMP_InputField Child2;
        public TMP_InputField Child3;

        [Header("Transitions")] public LeanPlayer AppearTransition;
        public LeanPlayer DisappearTransition;
    }

    public class SheetPresenter : PresenterBase<SheetView>
    {
        private readonly DataSheet _dataSheet;

        public SheetPresenter(SheetView view, DataSheet dataSheet) : base(view)
        {
            _dataSheet = dataSheet;
            InitDataSheet();
        }

        private void InitDataSheet()
        {
            _view.Name.SetTextWithoutNotify((_dataSheet.FilledData & PersonData.Name) == PersonData.Name
                ? _dataSheet.Name
                : string.Empty);

            var gender = (_dataSheet.FilledData & PersonData.Gender) == PersonData.Gender
                ? _dataSheet.Gender
                : Gender.None;
            var index = _view.Gender.options.FindIndex(o => o.text == gender.ToString());
            _view.Gender.SetValueWithoutNotify(index);

            _view.BDay.SetTextWithoutNotify((_dataSheet.FilledData & PersonData.Day) == PersonData.Day
                ? _dataSheet.DayOfBirth.ToString()
                : string.Empty);

            _view.BMonth.SetValueWithoutNotify((_dataSheet.FilledData & PersonData.Month) == PersonData.Month
                ? _dataSheet.MonthOfBirth
                : 0);

            _view.BYear.SetTextWithoutNotify((_dataSheet.FilledData & PersonData.Year) == PersonData.Year
                ? _dataSheet.YearOfBirth.ToString()
                : string.Empty);

            _view.PhoneNum.SetTextWithoutNotify((_dataSheet.FilledData & PersonData.PhoneNum) == PersonData.PhoneNum
                ? _dataSheet.PhoneNumber
                : string.Empty);

            _view.TaxNum.SetTextWithoutNotify((_dataSheet.FilledData & PersonData.TaxNum) == PersonData.TaxNum
                ? _dataSheet.TaxNumber
                : string.Empty);

            var city = (_dataSheet.FilledData & PersonData.AddressCity) == PersonData.AddressCity
                ? _dataSheet.AddressCity
                : "Not stated";
            var cityIndex = _view.City.options.FindIndex(o => o.text == city);
            _view.City.SetValueWithoutNotify(cityIndex);

            var street = (_dataSheet.FilledData & PersonData.AddressStreet) == PersonData.AddressStreet
                ? _dataSheet.AddressStreet
                : "Not stated";
            var streetIndex = _view.Street.options.FindIndex(o => o.text == street);
            _view.Street.SetValueWithoutNotify(streetIndex);

            _view.Bld.SetTextWithoutNotify((_dataSheet.FilledData & PersonData.AddressBld) == PersonData.AddressBld
                ? _dataSheet.AddressBld.ToString()
                : string.Empty);

            _view.PartnerName.SetTextWithoutNotify((_dataSheet.FilledData & PersonData.Spouse) == PersonData.Spouse
                ? _dataSheet.Spouse.Name
                : string.Empty);

            _view.PartnerPhone.SetTextWithoutNotify((_dataSheet.FilledData & PersonData.Spouse) == PersonData.Spouse
                ? _dataSheet.Spouse.PhoneNumber
                : string.Empty);

            if ((_dataSheet.FilledData & PersonData.Children) == PersonData.Children)
            {
                if (_dataSheet.ChildrenNames.Count > 0) _view.Child1.SetTextWithoutNotify(_dataSheet.ChildrenNames[0]);
                if (_dataSheet.ChildrenNames.Count > 1) _view.Child2.SetTextWithoutNotify(_dataSheet.ChildrenNames[1]);
                if (_dataSheet.ChildrenNames.Count > 2) _view.Child3.SetTextWithoutNotify(_dataSheet.ChildrenNames[2]);
            }
            
            _view.AppearTransition?.Begin();
        }

        public async UniTask Disappear()
        {
            _view.DisappearTransition.Begin();
            await UniTask.Delay(550);
        }
    }
}