namespace MaxFluff.Prototypes
{
    public abstract class PlayerView: TransformView
    {
        
    }

    public abstract class PlayerPresenter<T> : TransformPresenter<T> where T : PlayerView
    {
        public PlayerPresenter(T view): base(view)
        {
            
        }
    }
}