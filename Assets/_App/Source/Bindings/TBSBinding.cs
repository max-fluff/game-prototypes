namespace MaxFluff.Prototypes._App.Source.Bindings
{
    public class TBSBinding : IInitBinding
    {
        private readonly BoardPresenter _board;
        private readonly KeyboardInputService _keyboardInputService;

        public TBSBinding(
            BoardPresenter board,
            KeyboardInputService keyboardInputService)
        {
            _board = board;
            _keyboardInputService = keyboardInputService;
        }

        public void Init()
        {
            _keyboardInputService.OnInputAction += ProcessInput;
        }

        private void ProcessInput(Actions action)
        {
            if (action == Actions.Space)
            {
                if (_board.State == BoardState.Moving)
                    _board.State = BoardState.Action;
            }
        }
    }
}