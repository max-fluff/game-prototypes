using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = System.Random;

namespace MaxFluff.Prototypes
{
    public class TycoonBinding : IInitBinding, IStateChangerBinding
    {
        private readonly TycoonBarWindowPresenter _barWindowPresenter;
        private readonly TycoonGamePlaceWindowPresenter _gamePlaceWindowPresenter;
        private readonly TycoonResultsWindowPresenter _resultsWindowPresenter;
        private readonly TycoonSellWindowPresenter _sellWindowPresenter;
        private readonly MoneyCounterPresenter _moneyCounterPresenter;
        private readonly PopularityCounterPresenter _popularityCounterPresenter;
        private readonly ContinueButtonPresenter _continueButtonPresenter;
        private readonly MouseInputService _mouseInputService;
        private readonly FailWindowPresenter _failWindowPresenter;

        private float _popularity;
        private int _money;
        private int _lastProfit;
        private Random _rand;

        private bool _gotProtection;

        public event Action<IAppState> OnStateChangeRequested;

        private readonly List<IProfitablePresenter> _profitablePresenters = new List<IProfitablePresenter>();

        public TycoonBinding(
            TycoonBarWindowPresenter barWindowPresenter,
            TycoonGamePlaceWindowPresenter gamePlaceWindowPresenter,
            TycoonResultsWindowPresenter resultsWindowPresenter,
            TycoonSellWindowPresenter sellWindowPresenter,
            MoneyCounterPresenter moneyCounterPresenter,
            PopularityCounterPresenter popularityCounterPresenter,
            ContinueButtonPresenter continueButtonPresenter,
            MouseInputService mouseInputService,
            FailWindowPresenter failWindowPresenter
        )
        {
            _barWindowPresenter = barWindowPresenter;
            _gamePlaceWindowPresenter = gamePlaceWindowPresenter;
            _resultsWindowPresenter = resultsWindowPresenter;
            _sellWindowPresenter = sellWindowPresenter;
            _moneyCounterPresenter = moneyCounterPresenter;
            _popularityCounterPresenter = popularityCounterPresenter;
            _continueButtonPresenter = continueButtonPresenter;
            _mouseInputService = mouseInputService;
            _failWindowPresenter = failWindowPresenter;
        }

        private int Money
        {
            get => _money;
            set
            {
                _money = value;
                _moneyCounterPresenter.SetMoneyCounter(value);
                if (_money < 0)
                {
                    _failWindowPresenter.Open();
                }
            }
        }

        private float Popularity
        {
            get => _popularity;
            set
            {
                _popularity = value;
                _popularityCounterPresenter.UpdateValue(Mathf.Clamp01(value));
            }
        }

        public void Init()
        {
            _rand = new Random();

            Money = 8000;

            var views = Object.FindObjectsOfType<ProfitableView>();

            foreach (var view in views)
            {
                switch (view)
                {
                    case BarView barView:
                        var barPresenter = new BarPresenter(barView);
                        barPresenter.OnClicked += OpenBarWindow;
                        _profitablePresenters.Add(barPresenter);
                        break;
                    case GamePlaceView gamePlaceView:
                        var gamePlacePresenter = new GamePlacePresenter(gamePlaceView);
                        gamePlacePresenter.OnClicked += OpenGamePlaceWindow;
                        _profitablePresenters.Add(gamePlacePresenter);
                        break;
                }
            }

            Popularity = 0.1f;

            PreparationsAwaiter().Forget();

            _failWindowPresenter.OnClosed += Quit;
        }

        private void Quit()
        {
            OnStateChangeRequested?.Invoke(new StartScreenState());
        }

        private void OpenGamePlaceWindow(GamePlacePresenter gamePlacePresenter)
        {
            if (_mouseInputService.IsOverUI || _money <= 0) return;

            if (gamePlacePresenter.State == StationState.None)
            {
                _gamePlaceWindowPresenter.Open();

                _gamePlaceWindowPresenter.OnClosed += OnClosed;

                void OnClosed()
                {
                    gamePlacePresenter.State = _gamePlaceWindowPresenter.State;

                    Money -= gamePlacePresenter.GetBuyPrice();
                    Popularity += gamePlacePresenter.GetPopularityIncrease();

                    _gamePlaceWindowPresenter.OnClosed -= OnClosed;
                }
            }
            else
            {
                _sellWindowPresenter.SetPrice(gamePlacePresenter.GetSellPrice());
                _sellWindowPresenter.Open();

                _sellWindowPresenter.OnClosed += OnClosed;

                void OnClosed()
                {
                    if (_sellWindowPresenter.IsSelling)
                    {
                        Money += gamePlacePresenter.GetSellPrice();
                        Popularity -= gamePlacePresenter.GetPopularityIncrease();
                        gamePlacePresenter.State = StationState.None;
                    }

                    _sellWindowPresenter.OnClosed -= OnClosed;
                }
            }
        }

        private void OpenBarWindow(BarPresenter barPresenter)
        {
            if (_mouseInputService.IsOverUI || _money <= 0) return;

            if (barPresenter.IsBought)
            {
                _sellWindowPresenter.SetPrice(barPresenter.GetSellPrice());
                _sellWindowPresenter.Open();

                _sellWindowPresenter.OnClosed += OnClosed;

                void OnClosed()
                {
                    if (_sellWindowPresenter.IsSelling)
                    {
                        Money += barPresenter.GetSellPrice();
                        Popularity -= barPresenter.GetPopularityIncrease();
                        barPresenter.IsBought = false;
                    }

                    _sellWindowPresenter.OnClosed -= OnClosed;
                }
            }
            else
            {
                _barWindowPresenter.Open();

                _barWindowPresenter.OnClosed += OnClosed;

                void OnClosed()
                {
                    if (_barWindowPresenter.IsBuying)
                    {
                        Money -= barPresenter.GetBuyPrice();

                        barPresenter.IsBought = true;

                        Popularity += barPresenter.GetPopularityIncrease();
                    }

                    _barWindowPresenter.OnClosed -= OnClosed;
                }
            }
        }

        private async UniTask PreparationsAwaiter()
        {
            _continueButtonPresenter.SetActive(true);
            _continueButtonPresenter.OnContinue += ContinueScoped;

            var continues = false;
            void ContinueScoped() => continues = true;

            await UniTask.WaitUntil(() => continues);

            _continueButtonPresenter.OnContinue -= ContinueScoped;

            LevelAwaiter().Forget();
        }

        private async UniTask LevelAwaiter()
        {
            _continueButtonPresenter.SetActive(false);

            var profit = 0;
            var maintenanceCost = 0;

            foreach (var profitable in _profitablePresenters)
            {
                var players = profitable.GetPlayerAmount(_popularity);

                for (var i = 0; i < players; i++)
                {
                    var chanceOfRich = _lastProfit * 0.000005 * _popularity;
                    var chanceOfAverage = _lastProfit * 0.00003 * _popularity;
                    var chance = _rand.NextDouble();
                    int personMoneyAmount;
                    if (chance < chanceOfRich)
                        personMoneyAmount = _rand.Next(10000, 20000);
                    else if (chance < chanceOfAverage)
                        personMoneyAmount = _rand.Next(5000, 10001);
                    else
                        personMoneyAmount = _rand.Next(2000, 5000);

                    profit += (int) (personMoneyAmount * profitable.GetPercentOfIncome());
                }

                maintenanceCost += profitable.GetMaintenancePrice();
            }

            var tax = (int) (profit * 0.1f);

            Money += profit - tax - maintenanceCost;

            var timer = 1f;

            while (timer > 0)
            {
                await UniTask.NextFrame();
                timer -= Time.deltaTime;

                var raidChance = _rand.NextDouble();

                if (raidChance < (0.00005 + _lastProfit / 50000f) * Time.deltaTime)
                {
                    var percentageOfLose = (float) _rand.Next(40, 60);
                    var raidLost = (int) (_money * percentageOfLose / 100f);
                    Money -= raidLost;

                    _lastProfit = profit - tax - maintenanceCost - raidLost;

                    _resultsWindowPresenter.Open(profit, tax, maintenanceCost, raidLost, _lastProfit);

                    PreparationsAwaiter().Forget();
                    return;
                }
            }

            _resultsWindowPresenter.Open();

            _lastProfit = profit - tax - maintenanceCost;

            _resultsWindowPresenter.Open(profit, tax, maintenanceCost, 0, _lastProfit);

            PreparationsAwaiter().Forget();
        }
    }
}