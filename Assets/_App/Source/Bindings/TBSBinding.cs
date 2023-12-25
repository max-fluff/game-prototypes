namespace MaxFluff.Prototypes._App.Source.Bindings
{
    public class TBSBinding : IInitBinding
    {
        private readonly BoardPresenter _board;
        private readonly CursorService _cursorService;
        private readonly KeyboardInputService _keyboardInputService;

        public TBSBinding(
            BoardPresenter board,
            CursorService cursorService,
            KeyboardInputService keyboardInputService)
        {
            _board = board;
            _cursorService = cursorService;
            _keyboardInputService = keyboardInputService;
        }

        public void Init()
        {
            _keyboardInputService.OnInputAction += ProcessInput;
            _cursorService.IsCursorVisible = true;
        }

        private void ProcessInput(Actions action)
        {
            if (action == Actions.Shift)
            {
                if (_board.State == BoardState.Moving)
                    _board.State = BoardState.Action;
            }
        }
    }
}