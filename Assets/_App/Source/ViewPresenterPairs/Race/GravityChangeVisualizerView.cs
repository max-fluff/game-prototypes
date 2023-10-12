namespace MaxFluff.Prototypes
{
    public class GravityChangeVisualizerView : ViewBase
    {
    }

    public class GravityChangeVisualizerPresenter : PresenterBase<GravityChangeVisualizerView>
    {
        public GravityChangeVisualizerPresenter(GravityChangeVisualizerView view) : base(view)
        {
        }

        public void SetActive(bool isActive) => _view.gameObject.SetActive(isActive);
    }
}