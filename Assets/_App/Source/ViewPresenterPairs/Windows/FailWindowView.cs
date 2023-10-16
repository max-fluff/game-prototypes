using Lean.Gui;

namespace MaxFluff.Prototypes
{
    public class FailWindowView : WindowViewBase
    {
    }

    public class FailWindowPresenter : WindowPresenterBase<FailWindowView>
    {
        public override bool MayBeClosed => true;
        public override bool NeedBlocker => true;
        public override bool AllowAnotherWindowOnTop => true;
        public override bool ClosesOnOutsideClick => false;

        public FailWindowPresenter(FailWindowView view) : base(view)
        {
        }
    }
}