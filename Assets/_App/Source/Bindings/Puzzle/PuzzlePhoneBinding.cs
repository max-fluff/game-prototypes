namespace MaxFluff.Prototypes
{
    public class PuzzlePhoneBinding : IInitBinding
    {
        private readonly PhonePresenter _phone;
        private readonly ContactsList _contactsList;

        public PuzzlePhoneBinding(
            PhonePresenter phone,
            ContactsList contactsList
        )
        {
            _phone = phone;
            _contactsList = contactsList;
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
                var name = contacts[0].Name;
                _phone.SetName(name);
                _phone.SetState(PhoneState.ActiveCall);
            }
        }
    }
}