using Sirenix.Utilities;

namespace MaxFluff.Prototypes
{
    public class PuzzlePhoneBinding : IInitBinding
    {
        private readonly PhonePresenter _phone;
        private readonly ContactsList _contactsList;
        private readonly SheetPresenter _sheet;

        private PersonReaction _personReaction;

        public PuzzlePhoneBinding(
            PhonePresenter phone,
            ContactsList contactsList,
            SheetPresenter sheet
        )
        {
            _phone = phone;
            _contactsList = contactsList;
            _sheet = sheet;
        }

        public void Init()
        {
            _phone.OnCallRequested += Call;
        }

        private void Call(string number)
        {
            var contacts = _contactsList.Contacts.FindAll(c => c.PhoneNumber == number);
            if (contacts.Count > 0)
            {
                var contact = contacts[0];
                var name = contact.Name;
                _phone.SetName(name);
                var index = contact.PersonReactions.FindIndex(r => r.PhoneNumberToReact == _sheet.PhoneNumber);

                if (index >= 0)
                {
                    _personReaction = contact.PersonReactions[index];
                    var personData = PersonData.None;
                    contact.PersonReactions[index].Reactions.ForEach(r => personData |= r.DataReaction);
                    _phone.SetVisibleQuestions(personData);
                    _phone.SetLine(contact.InitReply);

                    _phone.OnQuestion += Question;
                    _phone.OnHungUp += HangUp;
                }
                else
                {
                    _phone.SetVisibleQuestions(PersonData.None);
                    _phone.SetLine(contact.InitNoDataReply.IsNullOrWhitespace()
                        ? "..."
                        : contact.InitNoDataReply);
                }

                _phone.SetState(PhoneState.ActiveCall);
            }
        }

        private void Question(PersonData personData)
        {
            var reaction = _personReaction.Reactions.Find(r => r.DataReaction == personData);
            _phone.SetLine(reaction.Reply);
        }

        private void HangUp()
        {
            _phone.OnQuestion -= Question;
            _phone.OnHungUp -= HangUp;
        }
    }
}