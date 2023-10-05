using System;

namespace MaxFluff.Prototypes
{
    public class PuzzleResultMarkView : ViewBase
    {
        public PersonData PersonData;
    }

    public class PuzzleResultMarkPresenter : PresenterBase<PuzzleResultMarkView>
    {
        public PersonData PersonData => _view.PersonData;
        
        public PuzzleResultMarkPresenter(PuzzleResultMarkView view) : base(view)
        {
        }
        
        public void SetVisible(bool isVisible) => _view.gameObject.SetActive(isVisible);
    }
}