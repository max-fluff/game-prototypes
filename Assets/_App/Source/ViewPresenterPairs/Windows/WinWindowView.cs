namespace MaxFluff.Prototypes
{
    public class WinWindowView : WindowViewBase
    {
    }

    public class WinWindowPresenter : WindowPresenterBase<WinWindowView>
    {
        public override bool MayBeClosed => true;
        public override bool NeedBlocker => true;
        public override bool AllowAnotherWindowOnTop => true;
        public override bool ClosesOnOutsideClick => false;

        public WinWindowPresenter(WinWindowView view) : base(view)
        {
        }
    }
}