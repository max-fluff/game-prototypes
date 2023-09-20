using UnityEngine;

namespace Omega.Kulibin
{
    public sealed class FPSBinding : IRunBinding, IInitBinding
    {
        private readonly Constants _constants;
        private readonly FpsCounterPresenter _fpsCounter;

        private float _timer;

        private float _fpsTimer;
        private float _framesCount;

        public FPSBinding(
            Constants constants,
            FpsCounterPresenter fpsCounter)
        {
            _constants = constants;
            _fpsCounter = fpsCounter;
        }

        public void Init()
        {
            _timer = Time.unscaledTime;
        }

        public void Run()
        {
            if (_fpsTimer >= 0.5f)
            {
                var avgFPS = _framesCount / _fpsTimer;
                _fpsCounter.UpdateFPS((int)avgFPS);

                if (Time.unscaledTime >= _timer)
                {
                    _timer += _constants.FpsRefreshRate;
                }

                _fpsTimer = 0;
                _framesCount = 0;
            }
            else
            {
                _fpsTimer += Time.unscaledDeltaTime;
                _framesCount++;
            }
        }
    }
}