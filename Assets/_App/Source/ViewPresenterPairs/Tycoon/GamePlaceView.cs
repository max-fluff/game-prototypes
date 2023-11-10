using System;
using EPOOutline;
using TMPro;
using UnityEngine;
using Random = System.Random;

namespace MaxFluff.Prototypes
{
    public class GamePlaceView : ProfitableView
    {
        public ClickableObject ClickableObject;

        public GameObject PokerGO;
        public GameObject BlackJackGO;
        public GameObject RouletteGO;
        public GameObject VacantGO;

        public Outlinable Outlinable;

        public TextMeshProUGUI PlayersAmount;
    }

    public class GamePlacePresenter : PresenterBase<GamePlaceView>, IProfitablePresenter
    {
        private readonly Random _random;
        private StationState _state;
        int _maxValue;

        public event Action<GamePlacePresenter> OnClicked;

        public GamePlacePresenter(GamePlaceView view) : base(view)
        {
            _random = new Random();
            _view.ClickableObject.OnClicked += SendOnClicked;
            State = StationState.None;

            _view.ClickableObject.OnHover += Hover;
            _view.ClickableObject.OnUnhover += UnHover;

            _view.Outlinable.OutlineLayer = 1;
        }

        private void Hover()
        {
            _view.Outlinable.OutlineLayer = 0;
        }

        private void UnHover()
        {
            _view.Outlinable.OutlineLayer = 1;
        }

        public StationState State
        {
            get => _state;
            set
            {
                _state = value;

                _view.RouletteGO.SetActive(value == StationState.Roulette);
                _view.PokerGO.SetActive(value == StationState.Poker);
                _view.BlackJackGO.SetActive(value == StationState.BlackJack);
                _view.VacantGO.SetActive(value == StationState.None);

                _maxValue = State switch
                {
                    StationState.None => 0,
                    StationState.Poker => 10,
                    StationState.BlackJack => 8,
                    StationState.Roulette => 20,
                    _ => throw new ArgumentOutOfRangeException()
                };

                _view.PlayersAmount.SetText($"{0}\\{_maxValue}");
            }
        }

        private void SendOnClicked() => OnClicked?.Invoke(this);

        public int GetSellPrice() =>
            State switch
            {
                StationState.Poker => 2500,
                StationState.BlackJack => 1500,
                StationState.Roulette => 7500,
                _ => throw new ArgumentOutOfRangeException()
            };

        public int GetBuyPrice() =>
            State switch
            {
                StationState.None => 0,
                StationState.Poker => 10000,
                StationState.BlackJack => 6000,
                StationState.Roulette => 25000,
                _ => throw new ArgumentOutOfRangeException()
            };

        public int GetMaintenancePrice() =>
            State switch
            {
                StationState.None => 0,
                StationState.Poker => 100,
                StationState.BlackJack => 1000,
                StationState.Roulette => 5000,
                _ => throw new ArgumentOutOfRangeException()
            };

        public int GetPlayerAmount(float popularity)
        {
            var amount = State switch
            {
                StationState.None => 0,
                StationState.Poker => (int)Mathf.Clamp(_random.Next(2, 5) + popularity * 7, 2, 10),
                StationState.BlackJack => (int)Mathf.Clamp(_random.Next(2, 4) + popularity * 6, 2, 8),
                StationState.Roulette => (int)Mathf.Clamp(_random.Next(1, 10) + popularity * 15, 1, 20),
                _ => throw new ArgumentOutOfRangeException()
            };

            _view.PlayersAmount.SetText($"{amount}\\{_maxValue}");

            return amount;
        }

        public float GetPopularityIncrease() =>
            State switch
            {
                StationState.None => 0,
                StationState.Poker => 0.02f,
                StationState.BlackJack => 0.05f,
                StationState.Roulette => 0.1f,
                _ => throw new ArgumentOutOfRangeException()
            };

        public float GetPercentOfIncome()
        {
            var min = State switch
            {
                StationState.None => 0,
                StationState.Poker => 5,
                StationState.BlackJack => 10,
                StationState.Roulette => 25,
                _ => throw new ArgumentOutOfRangeException()
            };
            var max = State switch
            {
                StationState.None => 0,
                StationState.Poker => 15,
                StationState.BlackJack => 30,
                StationState.Roulette => 25,
                _ => throw new ArgumentOutOfRangeException()
            };

            return _random.Next(min, max) / 100f;
        }
    }

    public enum StationState
    {
        None,
        Poker,
        BlackJack,
        Roulette
    }
}