using UnityEngine;

namespace MaxFluff.Prototypes
{
    public class StartScreenQuitBinding : IInitBinding
    {
        private readonly StartScreenPresenter _startScreenPresenter;

        public StartScreenQuitBinding(
            StartScreenPresenter startScreenPresenter
        )
        {
            _startScreenPresenter = startScreenPresenter;
        }

        public void Init()
        {
            _startScreenPresenter.OnQuitClicked += Quit;
        }

        private void Quit() => Application.Quit();
    }
}