using UnityEngine;
using UnityEngine.UI;

namespace MaxFluff.Prototypes
{
    public class PuzzleResultMarkView : ViewBase
    {
        public PersonData PersonData;
        public Sprite Positive;
        public Color PositiveColor;
        public Sprite Negative;
        public Color NegativeColor;
    }

    public class PuzzleResultMarkPresenter : PresenterBase<PuzzleResultMarkView>
    {
        public PersonData PersonData => _view.PersonData;

        private readonly Image _image;

        public PuzzleResultMarkPresenter(PuzzleResultMarkView view) : base(view)
        {
            _image = _view.GetComponent<Image>();
        }

        public void SetVisible(bool isVisible) => _view.gameObject.SetActive(isVisible);

        public void SetState(bool isPositive)
        {
            _image.sprite = isPositive ? _view.Positive : _view.Negative;
            _image.color = isPositive ? _view.PositiveColor : _view.NegativeColor;
            _view.gameObject.SetActive(isPositive);
        }
    }
}