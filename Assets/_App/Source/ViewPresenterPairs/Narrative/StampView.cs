namespace MaxFluff.Prototypes
{
    public class StampView : DraggableObjectView
    {
    }

    public class StampPresenter : DraggableObjectPresenter<StampView>
    {
        public StampPresenter(StampView view) : base(view)
        {
        }
    }
}