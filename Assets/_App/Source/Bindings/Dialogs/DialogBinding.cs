using Cysharp.Threading.Tasks;

namespace MaxFluff.Prototypes
{
    public class DialogBinding : IInitBinding
    {
        private readonly Dialogue _dialogue;
        private readonly ReplyPresenter _replyPresenter;
        private readonly CurrentLinePresenter _currentLinePresenter;
        private readonly CounterPresenter _counterPresenter;
        private readonly PersonPresenter _personPresenter;

        private int _currentLine = -1;
        private int _results = 3;

        public DialogBinding(
            Dialogue dialogue,
            ReplyPresenter replyPresenter,
            CurrentLinePresenter currentLinePresenter,
            CounterPresenter counterPresenter,
            PersonPresenter personPresenter)
        {
            _dialogue = dialogue;
            _replyPresenter = replyPresenter;
            _currentLinePresenter = currentLinePresenter;
            _counterPresenter = counterPresenter;
            _personPresenter = personPresenter;
        }

        public void Init()
        {
            _counterPresenter.SetCount(_results);
            ShowNextLine();
            _replyPresenter.OnReply += ShowNextLine;
        }

        private void ShowNextLine() => ShowNextLineAsync().Forget();

        private async UniTask ShowNextLineAsync()
        {
            _currentLine++;

            if (_currentLine >= _dialogue.Lines.Count)
            {
                _currentLinePresenter.SetText("Was nice to talk to you! Bye!");
                _replyPresenter.SetActive(false);
                _personPresenter.StartTalking();
                return;
            }

            var dialogueLine = _dialogue.Lines[_currentLine];

            if (dialogueLine.variants.Count == 0)
            {
                await ProcessLineVariant(dialogueLine.defaultPoints, dialogueLine.defaultLine);
            }
            else
            {
                var result = _replyPresenter.GetReply();

                var wasFound = false;

                foreach (var variant in dialogueLine.variants)
                {
                    if (result.x >= (variant.positivityRange.x - 0.001f) &&
                        result.x <= (variant.positivityRange.y + 0.001f)
                        && result.y >= (variant.passionRange.x - 0.001f) &&
                        result.y <= (variant.passionRange.y + 0.001f)
                        && result.z >= (variant.confidenceRange.x - 0.001f) &&
                        result.z <= (variant.confidenceRange.y + 0.001f)
                        && result.w >= (variant.smartRange.x - 0.001f) &&
                        result.w <= (variant.smartRange.y + 0.001f))
                    {
                        await ProcessLineVariant(variant.points, variant.line);

                        wasFound = true;

                        break;
                    }
                }

                if (!wasFound)
                    await ProcessLineVariant(dialogueLine.defaultPoints, dialogueLine.defaultLine);


                if (_currentLine == _dialogue.Lines.Count - 1)
                {
                    _currentLinePresenter.SetText("Was nice to talk to you! Bye!");
                    _replyPresenter.SetActive(false);
                }
                else if (_dialogue.Lines[_currentLine + 1].variants.Count == 0 && _results > 0)
                    ShowNextLine();
            }
        }

        private async UniTask ProcessLineVariant(int points, string line)
        {
            _results += points;


            _personPresenter.UpdateForm(_results / 10f);

            _currentLinePresenter.SetText(line);
            _counterPresenter.SetCount(_results <= 0 ? 0 : _results);

            _replyPresenter.SetActive(false);

            _personPresenter.StartTalking();

            await UniTask.Delay(2000);

            if (_results > 0)
                _replyPresenter.SetActive(true);
            else
            {
                _counterPresenter.SetCount(0);

                _currentLinePresenter.SetText("Man, you act strange today... Maybe next time...");

                _personPresenter.StartTalking();

                await UniTask.Delay(2000);

                _currentLinePresenter.SetText("...");
                _personPresenter.SetActive(false);
            }
        }
    }
}