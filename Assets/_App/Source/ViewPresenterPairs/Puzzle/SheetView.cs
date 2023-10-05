using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Lean.Transition;
using TMPro;
using UnityEngine;

namespace MaxFluff.Prototypes
{
    public class SheetView : ViewBase
    {
        public List<PuzzleResultMarkView> ResultMarks = new List<PuzzleResultMarkView>();

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

        public GameObject Congrats;
        public GameObject Main;

        [Header("Transitions")] public LeanPlayer AppearTransition;
        public LeanPlayer DisappearTransition;
    }

    public class SheetPresenter : PresenterBase<SheetView>
    {
        private DataSheet _dataSheet;
        public List<PuzzleResultMarkPresenter> ResultMarks = new List<PuzzleResultMarkPresenter>();

        public SheetPresenter(SheetView view) : base(view)
        {
            foreach (var resultMarkPresenter in view.ResultMarks.Select(resultMarkView =>
                         new PuzzleResultMarkPresenter(resultMarkView)))
            {
                ResultMarks.Add(resultMarkPresenter);
                resultMarkPresenter.SetVisible(false);
            }
        }

        public string PhoneNumber => _dataSheet.PhoneNumber;

        public void ShowCongrats()
        {
            _view.Congrats.SetActive(true);
            _view.Main.SetActive(false);
            _view.AppearTransition?.Begin();
            _dataSheet = default;
        }

        public void UpdateDataSheet(DataSheet dataSheet)
        {
            _dataSheet = dataSheet;

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

        public void ShowResult()
        {
            var personData = PersonData.None;

            if (_dataSheet.Name == _view.Name.text)
                personData |= PersonData.Name;
            if (_dataSheet.Gender.ToString() == _view.Gender.options[_view.Gender.value].text)
                personData |= PersonData.Gender;
            if (_dataSheet.DayOfBirth.ToString() == _view.BDay.text)
                personData |= PersonData.Day;
            if (_dataSheet.MonthOfBirth == _view.BMonth.value)
                personData |= PersonData.Month;
            if (_dataSheet.YearOfBirth.ToString() == _view.BYear.text)
                personData |= PersonData.Year;

            if (_dataSheet.PhoneNumber == _view.PhoneNum.text)
                personData |= PersonData.PhoneNum;
            if (_dataSheet.TaxNumber == _view.TaxNum.text)
                personData |= PersonData.TaxNum;

            if (_dataSheet.AddressCity == _view.City.options[_view.City.value].text)
                personData |= PersonData.AddressCity;
            if (_dataSheet.AddressStreet == _view.Street.options[_view.Street.value].text)
                personData |= PersonData.AddressStreet;
            if (_dataSheet.AddressBld.ToString() == _view.Bld.text)
                personData |= PersonData.AddressBld;

            if (_dataSheet.Spouse.Name == _view.PartnerName.text &&
                _dataSheet.Spouse.PhoneNumber == _view.PartnerPhone.text)
                personData |= PersonData.Spouse;

            var shouldAddChildren = true;

            foreach (var name in _dataSheet.ChildrenNames)
            {
                if ((_view.Child1.text != name && _view.Child2.text != name && _view.Child3.text != name) ||
                    _view.Child1.text == _view.Child2.text
                    || _view.Child1.text == _view.Child3.text
                    || _view.Child2.text == _view.Child3.text)
                {
                    shouldAddChildren = false;
                    break;
                }
            }

            if (shouldAddChildren)
                personData |= PersonData.Children;

            foreach (var resultMark in ResultMarks)
                resultMark.SetVisible((resultMark.PersonData & personData) == resultMark.PersonData);
        }

        public async UniTask Disappear()
        {
            _view.DisappearTransition.Begin();
            await UniTask.Delay(550);
            foreach (var resultMark in ResultMarks)
                resultMark.SetVisible(false);
        }
    }
}