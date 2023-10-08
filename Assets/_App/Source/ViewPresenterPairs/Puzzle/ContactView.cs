using System;
using Lean.Gui;
using TMPro;

namespace MaxFluff.Prototypes
{
    public class ContactView : ViewBase
    {
        public TextMeshProUGUI Name;
        public TextMeshProUGUI Number;
        public LeanButton Call;
    }

    public class ContactPresenter : PresenterBase<ContactView>
    {
        public event Action<string> OnCall;

        public ContactPresenter(ContactView view, string name, string number) : base(view)
        {
            _view.Name.text = name;
            _view.Number.text = number;
            _view.Call.OnClick.AddListener(Call);
        }

        private void Call()
        {
            OnCall?.Invoke(_view.Number.text);
        }
    }
}