using Lean.Gui;
using Lean.Transition;
using UnityEngine;

namespace MaxFluff.Prototypes
{
    public class PersonView : ViewBase
    {
        public LeanPlayer TalkTransition;
        public LeanBox Shape;
    }

    public class PersonPresenter : PresenterBase<PersonView>
    {
        public PersonPresenter(PersonView view) : base(view)
        {
        }

        public void StartTalking()
        {
            _view.TalkTransition.Begin();
        }

        public void UpdateForm(float value)
        {
            value = Mathf.Clamp(value, 0, 1);
            var size = ((RectTransform)_view.Shape.transform).sizeDelta.x;
            _view.Shape.Radius = size / 2 * value;
        }

        public void SetActive(bool isActive) =>
            _view.gameObject.SetActive(isActive);
    }
}