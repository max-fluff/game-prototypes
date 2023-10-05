using System;
using System.Collections.Generic;
using Lean.Gui;
using TMPro;
using UnityEngine;
using Object = UnityEngine.Object;

namespace MaxFluff.Prototypes
{
    public class PhoneView : ViewBase
    {
        [Header("ActiveCall")] public GameObject ActiveCallState;

        public TextMeshProUGUI Name;
        public TextMeshProUGUI Line;
        public LeanButton NameQuestion;
        public LeanButton GenderQuestion;
        public LeanButton DoBQuestion;
        public LeanButton PhoneNumQuestion;
        public LeanButton TaxNumQuestion;
        public LeanButton AddressQuestion;
        public LeanButton PartnerQuestion;
        public LeanButton ChildrenQuestion;
        public LeanButton HangUp;

        [Header("PhoneBook")] public GameObject PhoneBookState;
        public Transform PhoneBookContent;
        public ContactView ContactPrefab;
        public LeanButton ShowDialButton;

        [Header("Dial")] public GameObject DialState;
        public TMP_InputField PhoneInputField;
        public LeanButton PhoneCallButton;
        public LeanButton ShowPhoneBookButton;
    }

    public class PhonePresenter : PresenterBase<PhoneView>
    {
        public event Action<PersonData> OnQuestion;
        public event Action<string> OnCallRequested;

        private List<ContactPresenter> _contacts = new List<ContactPresenter>();

        public PhonePresenter(PhoneView view, ContactsList contactsList) : base(view)
        {
            foreach (var contact in contactsList.Contacts)
            {
                var contactView = Object.Instantiate(view.ContactPrefab, view.PhoneBookContent);
                var contactPresenter = new ContactPresenter(contactView, contact.Name, contact.PhoneNumber);
                contactPresenter.OnCall += Call;
                _contacts.Add(contactPresenter);
            }

            /* view.NameQuestion.OnClick.AddListener(SendNameQuestion);
             view.GenderQuestion.OnClick.AddListener(SendGenderQuestion);
             view.DoBQuestion.OnClick.AddListener(SendDoBQuestion);
             view.PhoneNumQuestion.OnClick.AddListener(SendPhoneNumQuestion);
             view.TaxNumQuestion.OnClick.AddListener(SendTaxNumQuestion);
             view.AddressQuestion.OnClick.AddListener(SendAddressQuestion);
             view.PartnerQuestion.OnClick.AddListener(SendPartnerQuestion);
             view.ChildrenQuestion.OnClick.AddListener(SendChildrenQuestion);*/

            view.PhoneCallButton.OnClick.AddListener(CallFromPhone);

            view.HangUp.OnClick.AddListener(HangUp);
            view.ShowDialButton.OnClick.AddListener(SetDialState);
            view.ShowPhoneBookButton.OnClick.AddListener(SetPhoneBookState);
            SetDialState();
        }

        private void SetPhoneBookState() => SetState(PhoneState.PhoneBook);
        private void SetDialState() => SetState(PhoneState.Dial);

        private void SendNameQuestion() => OnQuestion?.Invoke(PersonData.Name);
        private void SendGenderQuestion() => OnQuestion?.Invoke(PersonData.Gender);
        private void SendDoBQuestion() => OnQuestion?.Invoke(PersonData.BirthDay);
        private void SendPhoneNumQuestion() => OnQuestion?.Invoke(PersonData.PhoneNum);
        private void SendTaxNumQuestion() => OnQuestion?.Invoke(PersonData.TaxNum);
        private void SendAddressQuestion() => OnQuestion?.Invoke(PersonData.Address);
        private void SendPartnerQuestion() => OnQuestion?.Invoke(PersonData.Spouse);
        private void SendChildrenQuestion() => OnQuestion?.Invoke(PersonData.Children);

        public void SetName(string name) => _view.Name.SetText(name);
        public void SetLine(string line) => _view.Line.SetText(line);

        private void CallFromPhone() => Call(_view.PhoneInputField.text);
        private void Call(string number) => OnCallRequested?.Invoke(number);

        public void HangUp() =>
            SetDialState();

        public void SetState(PhoneState state)
        {
            _view.ActiveCallState.SetActive(state == PhoneState.ActiveCall);
            _view.PhoneBookState.SetActive(state == PhoneState.PhoneBook);
            _view.DialState.SetActive(state == PhoneState.Dial);
        }
    }

    public enum PhoneState
    {
        ActiveCall,
        PhoneBook,
        Dial
    }
}