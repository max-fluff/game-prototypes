namespace MaxFluff.Prototypes
{
    public class PenView: DraggableObjectView
    {
        
    }
    
    public class PenPresenter: DraggableObjectPresenter<PenView>
    {
        public PenPresenter(PenView view) : base(view)
        {
        }
    }
}