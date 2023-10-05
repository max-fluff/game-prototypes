using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Lean.Gui;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace MaxFluff.Prototypes
{
    public class PhoneView : ViewBase
    {
        [Header("ActiveCall")] public GameObject ActiveCallState;

        public TextMeshProUGUI Name;
        public TextMeshProUGUI Line;
        public List<QuestionButtonView> Questions;
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
        public event Action OnHungUp;

        private PhoneState _cachedState;

        private List<ContactPresenter> _contacts = new List<ContactPresenter>();
        private List<QuestionButtonPresenter> _questions = new List<QuestionButtonPresenter>();

        public PhonePresenter(PhoneView view, ContactsList contactsList) : base(view)
        {
            foreach (var contact in contactsList.Contacts.OrderBy(c => c.Name))
            {
                var contactView = Object.Instantiate(view.ContactPrefab, view.PhoneBookContent);
                var contactPresenter = new ContactPresenter(contactView, contact.Name, contact.PhoneNumber);
                contactPresenter.OnCall += Call;
                _contacts.Add(contactPresenter);
            }

            foreach (var questionButtonView in _view.Questions)
            {
                var questionButtonPresenter = new QuestionButtonPresenter(questionButtonView);
                questionButtonPresenter.OnClicked += SendQuestion;
                _questions.Add(questionButtonPresenter);
            }

            view.PhoneCallButton.OnClick.AddListener(CallFromPhone);

            view.HangUp.OnClick.AddListener(HangUp);
            view.ShowDialButton.OnClick.AddListener(SetDialState);
            view.ShowPhoneBookButton.OnClick.AddListener(SetPhoneBookState);
            SetDialState();
        }

        private void SetPhoneBookState() => SetState(PhoneState.PhoneBook);
        private void SetDialState() => SetState(PhoneState.Dial);

        private void SendQuestion(PersonData personData) => OnQuestion?.Invoke(personData);

        public void SetName(string name) => _view.Name.SetText(name);

        public void SetVisibleQuestions(PersonData availableQuestions)
        {
            foreach (var question in _questions)
                question.SetVisible((question.PersonData & availableQuestions) == question.PersonData);
            Rebuild().Forget();
        }

        private async UniTask Rebuild()
        {
            await UniTask.Yield();
            LayoutRebuilder.MarkLayoutForRebuild((RectTransform) _view.HangUp.transform);
            LayoutRebuilder.MarkLayoutForRebuild((RectTransform) _view.HangUp.transform.parent);
        }

        public void SetLine(string line)
        {
            _view.Line.SetText(line);
            Rebuild().Forget();
        }

        private void CallFromPhone() => Call(_view.PhoneInputField.text);
        private void Call(string number) => OnCallRequested?.Invoke(number);

        public void HangUp()
        {
            OnHungUp?.Invoke();
            SetState(_cachedState);
        }

        public void SetState(PhoneState state)
        {
            _cachedState = state == PhoneState.ActiveCall
                ? _cachedState
                : state;
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