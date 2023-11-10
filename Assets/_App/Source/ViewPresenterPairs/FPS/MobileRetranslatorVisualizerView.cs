namespace MaxFluff.Prototypes.FPS
{
    public class MobileRetranslatorVisualizerView : ViewBase
    {
    }

    public class MobileRetranslatorVisualizerPresenter : PresenterBase<MobileRetranslatorVisualizerView>
    {
        public MobileRetranslatorVisualizerPresenter(MobileRetranslatorVisualizerView view) : base(view)
        {
        }

        public void Enable()
        {
            _view.gameObject.SetActive(true);
        }
    }
}