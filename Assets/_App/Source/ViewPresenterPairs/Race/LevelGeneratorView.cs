using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = System.Random;

namespace MaxFluff.Prototypes
{
    public class LevelGeneratorView : ViewBase
    {
        public LevelElementView StartingElement;
        public List<LevelElementView> ElementPrefabs;
        public FinishView FinishView;
    }

    public class LevelGeneratorPresenter : PresenterBase<LevelGeneratorView>
    {
        private List<LevelElementPresenter> _levelElements = new List<LevelElementPresenter>();

        private readonly Random _random;

        public event Action<Vector3> OnRequestedPlayerMove;

        public LevelGeneratorPresenter(LevelGeneratorView view) : base(view)
        {
            _random = new Random();

            GenerateNewLevel();
        }

        public void GenerateNewLevel()
        {
            foreach (var levelElement in _levelElements) levelElement.Destroy();

            var lastElement = new LevelElementPresenter(_view.StartingElement);

            _levelElements = GenerateLevel(lastElement, 10);
            _levelElements[4].OnPlayerTriggered += GenerateFiveMore;
            _levelElements[9].OnPlayerTriggered += GenerateFiveMore;
            _levelElements[9].OnPlayerTriggered += DestroyFirstFive;

            _view.FinishView.transform.position = _levelElements[_levelElements.Count - 1].GetNextConnectionPoint();
        }

        private List<LevelElementPresenter> GenerateLevel(LevelElementPresenter lastElement, int n)
        {
            var list = new List<LevelElementPresenter>();
            for (var i = 0; i < n; i++)
            {
                var nextConnectionPoint = lastElement.GetNextConnectionPoint();
                var nextElementNumber = _random.Next(_view.ElementPrefabs.Count);

                var nextElement = Object.Instantiate(_view.ElementPrefabs[nextElementNumber], _view.transform, true);
                var nextElementPresenter = new LevelElementPresenter(nextElement);

                var rotationAmount = _random.Next(0, 4);
                nextElementPresenter.RotateAroundW(rotationAmount);

                var positionDelta = nextElementPresenter.GetPosition() - nextConnectionPoint +
                                    nextElementPresenter.GetPreviousConnectionPoint();

                nextElementPresenter.Move(-positionDelta);

                list.Add(nextElementPresenter);

                lastElement = nextElementPresenter;
            }

            return list;
        }

        private void GenerateFiveMore()
        {
            var newList = GenerateLevel(_levelElements.Last(), 5);
            newList.Last().OnPlayerTriggered += GenerateFiveMore;
            newList.Last().OnPlayerTriggered += DestroyFirstFive;

            _levelElements.AddRange(newList);

            _view.FinishView.transform.position = _levelElements[_levelElements.Count - 1].GetNextConnectionPoint();
        }

        private void DestroyFirstFive()
        {
            for (var i = 0; i < 5; i++)
            {
                _levelElements[0].Destroy();
                _levelElements.RemoveAt(0);
            }

            var startingPointConnectionPoint = _view.StartingElement.NextConnectionJoint.position;

            var positionDelta = -startingPointConnectionPoint + _levelElements[0].GetPreviousConnectionPoint();

            foreach (var levelElement in _levelElements)
                levelElement.Move(-positionDelta);

            _view.FinishView.transform.position -= positionDelta;

            OnRequestedPlayerMove?.Invoke(-positionDelta);
        }
    }
}