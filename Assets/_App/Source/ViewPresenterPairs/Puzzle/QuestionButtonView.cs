using System;

namespace MaxFluff.Prototypes
{
    public class QuestionButtonView : ButtonBaseView
    {
        public PersonData PersonData;
    }

    public class QuestionButtonPresenter : ButtonBasePresenter<QuestionButtonView>
    {
        public PersonData PersonData => _view.PersonData;

        public event Action<PersonData> OnClicked;

        public QuestionButtonPresenter(QuestionButtonView view) : base(view) => OnClick += SendOnClicked;

        private void SendOnClicked() => OnClicked?.Invoke(PersonData);

        public void SetVisible(bool isVisible) => _view.gameObject.SetActive(isVisible);
    }
}