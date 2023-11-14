using System;
using System.Collections.Generic;
using Object = UnityEngine.Object;

namespace MaxFluff.Prototypes
{
    public class LevelGeneratorView : ViewBase
    {
        public LevelElementView StartingElement;
        public List<LevelElementView> ElementPrefabs;
    }

    public class LevelGeneratorPresenter : PresenterBase<LevelGeneratorView>
    {
        private List<LevelElementPresenter> _levelElements = new List<LevelElementPresenter>();

        private readonly Random _random;

        public LevelGeneratorPresenter(LevelGeneratorView view) : base(view)
        {
            _random = new Random();

            GenerateLevel();
        }

        public void GenerateLevel()
        {
            foreach (var levelElement in _levelElements) levelElement.Destroy();
            _levelElements.Clear();

            var lastElement = new LevelElementPresenter(_view.StartingElement);

            for (var i = 0; i < 10; i++)
            {
                var nextConnectionPoint = lastElement.GetNextConnectionPoint();
                var nextElementNumber = _random.Next(_view.ElementPrefabs.Count);

                var nextElement = Object.Instantiate(_view.ElementPrefabs[nextElementNumber]);
                var nextElementPresenter = new LevelElementPresenter(nextElement);

                var rotationAmount = _random.Next(0, 4);
                nextElementPresenter.RotateAroundW(rotationAmount * 90f);

                var positionDelta = nextElementPresenter.GetPosition() - nextConnectionPoint +
                                    nextElementPresenter.GetPreviousConnectionPoint();

                nextElementPresenter.Move(-positionDelta);

                _levelElements.Add(nextElementPresenter);

                lastElement = nextElementPresenter;
            }
        } }
}