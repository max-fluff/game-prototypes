using MaxFluff.Prototypes.FPS;

namespace MaxFluff.Prototypes
{
    public class CoreBinding : IInitBinding
    {
        private readonly CorePresenter _corePresenter;

        private int _burnPiecesAmount = 0;

        public CoreBinding(
            CorePresenter corePresenter
        )
        {
            _corePresenter = corePresenter;
        }

        public void Init()
        {
            _corePresenter.OnZapped += ProcessCoreDestruction;
        }

        private void ProcessCoreDestruction()
        {
            _burnPiecesAmount++;
            var piecesAmount = _corePresenter.GetCorePiecesAmount();
        }
    }
}