using System;
using TMPro;

namespace MaxFluff.Prototypes
{
    public class TimeResultWindowView : WindowViewBase
    {
        public TextMeshProUGUI Result;
    }

    public class TimeResultWindowPresenter : WindowPresenterBase<TimeResultWindowView>
    {
        public override bool MayBeClosed => true;
        public override bool NeedBlocker => true;
        public override bool AllowAnotherWindowOnTop => true;
        public override bool ClosesOnOutsideClick => false;

        public TimeResultWindowPresenter(TimeResultWindowView view) : base(view)
        {
        }

        public void SetTime(float seconds)
        {
            var time = TimeSpan.FromSeconds(seconds);
            _view.Result.SetText($"{time.Minutes}:{time.Seconds}:{time.Milliseconds}");
        }
    }
}