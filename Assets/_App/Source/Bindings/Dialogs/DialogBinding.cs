namespace MaxFluff.Prototypes
{
    public class DialogBinding : IInitBinding
    {
        private readonly Dialogue _dialogue;
        private readonly ReplyPresenter _replyPresenter;
        private readonly CurrentLinePresenter _currentLinePresenter;
        private readonly CounterPresenter _counterPresenter;

        public DialogBinding(
            Dialogue dialogue,
            ReplyPresenter replyPresenter,
            CurrentLinePresenter currentLinePresenter,
            CounterPresenter counterPresenter)
        {
            _dialogue = dialogue;
            _replyPresenter = replyPresenter;
            _currentLinePresenter = currentLinePresenter;
            _counterPresenter = counterPresenter;
        }

        public void Init()
        {
        }
    }
}