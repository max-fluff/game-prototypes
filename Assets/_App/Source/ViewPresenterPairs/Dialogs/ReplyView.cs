using Lean.Gui;
using UnityEngine;
using UnityEngine.UI;

namespace MaxFluff.Prototypes
{
    public class ReplyView : ViewBase
    {
        public Slider FormSlider;
        public Slider RSlider;
        public Slider GSlider;
        public Slider BSlider;
        public LeanBox Shape;
    }

    public class ReplyPresenter : PresenterBase<ReplyView>
    {
        public ReplyPresenter(ReplyView view) : base(view)
        {
            _view.FormSlider.onValueChanged.AddListener(UpdateForm);
            _view.RSlider.onValueChanged.AddListener(UpdateR);
            _view.GSlider.onValueChanged.AddListener(UpdateG);
            _view.BSlider.onValueChanged.AddListener(UpdateB);


            UpdateForm(_view.FormSlider.value);
            UpdateR(_view.RSlider.value);
            UpdateG(_view.GSlider.value);
            UpdateB(_view.BSlider.value);
        }

        public void UpdateForm(float value)
        {
            var size = ((RectTransform)_view.Shape.transform).sizeDelta.x;
            _view.Shape.Radius = size / 2 * value;
        }

        public void UpdateR(float value)
        {
            var color = _view.Shape.color;
            color.r = value;
            _view.Shape.color = color;
        }

        public void UpdateG(float value)
        {
            var color = _view.Shape.color;
            color.g = value;
            _view.Shape.color = color;
        }

        public void UpdateB(float value)
        {
            var color = _view.Shape.color;
            color.b = value;
            _view.Shape.color = color;
        }

        public Vector4 GetReply()
        {
            return new Vector4(_view.FormSlider.value, _view.RSlider.value, _view.GSlider.value, _view.BSlider.value);
        }
    }
}