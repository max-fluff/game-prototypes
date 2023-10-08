namespace MaxFluff.Prototypes
{
    public class SendButtonView : ButtonBaseView
    {
    }

    public class SendButtonPresenter : ButtonBasePresenter<SendButtonView>
    {
        public SendButtonPresenter(SendButtonView view) : base(view)
        {
        }
    }
}