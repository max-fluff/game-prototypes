using System;
using Lean.Gui;
using UnityEngine;
using UnityEngine.UI;

namespace MaxFluff.Prototypes
{
    public class ReplyView : ViewBase
    {
        public Slider PositiveSlider;
        public Slider PassionSlider;
        public Slider ConfidenceSlider;
        public Slider SmartSlider;
        public LeanBox Shape;
        public LeanButton Reply;
    }

    public class ReplyPresenter : PresenterBase<ReplyView>
    {
        public event Action OnReply;

        public ReplyPresenter(ReplyView view) : base(view)
        {
            _view.PositiveSlider.onValueChanged.AddListener(UpdateForm);
            _view.PassionSlider.onValueChanged.AddListener(UpdateR);
            _view.ConfidenceSlider.onValueChanged.AddListener(UpdateB);
            _view.SmartSlider.onValueChanged.AddListener(UpdateG);

            UpdateForm(_view.PositiveSlider.value);
            UpdateR(_view.PassionSlider.value);
            UpdateG(_view.ConfidenceSlider.value);
            UpdateB(_view.SmartSlider.value);

            _view.Reply.OnClick.AddListener(SendOnReply);
        }

        private void SendOnReply()
        {
            OnReply?.Invoke();

            _view.PositiveSlider.value = 0f;
            _view.PassionSlider.value = 0f;
            _view.ConfidenceSlider.value = 0f;
            _view.SmartSlider.value = 0f;
        }

        private void UpdateForm(float value)
        {
            var size = ((RectTransform)_view.Shape.transform).sizeDelta.x;
            _view.Shape.Radius = size / 2 * value;
        }

        private void UpdateR(float value)
        {
            var color = _view.Shape.color;
            color.r = value;
            _view.Shape.color = color;
        }

        private void UpdateG(float value)
        {
            var color = _view.Shape.color;
            color.g = value;
            _view.Shape.color = color;
        }

        private void UpdateB(float value)
        {
            var color = _view.Shape.color;
            color.b = value;
            _view.Shape.color = color;
        }

        public Vector4 GetReply() =>
            new Vector4(_view.PositiveSlider.value, _view.PassionSlider.value, _view.ConfidenceSlider.value, _view.SmartSlider.value);

        public void SetActive(bool isActive) =>
            _view.gameObject.SetActive(isActive);
    }
}