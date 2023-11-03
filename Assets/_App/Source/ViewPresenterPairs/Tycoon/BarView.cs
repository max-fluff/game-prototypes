using System;
using EPOOutline;
using UnityEngine;
using Random = System.Random;

namespace MaxFluff.Prototypes
{
    public class BarView : ProfitableView
    {
        public ClickableObject ClickableObject;
        public GameObject VisualsGO;
        public GameObject BuyVisualsGO;
        public Outlinable Outlinable;
    }

    public class BarPresenter : PresenterBase<BarView>, IProfitablePresenter
    {
        private readonly Random _random;
        private bool _isBought;
        public event Action<BarPresenter> OnClicked;

        public bool IsBought
        {
            get => _isBought;
            set
            {
                _isBought = value;
                _view.VisualsGO.SetActive(value);
                _view.BuyVisualsGO.SetActive(!value);
            }
        }

        public BarPresenter(BarView view) : base(view)
        {
            _random = new Random();
            _view.ClickableObject.OnClicked += SendOnClicked;
            _view.ClickableObject.OnHover += Hover;
            _view.ClickableObject.OnUnhover += UnHover;
            IsBought = false;

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

        private void SendOnClicked() => OnClicked?.Invoke(this);

        public int GetSellPrice() => IsBought ? 500 : throw new NotImplementedException();
        public int GetBuyPrice() => IsBought ? throw new NotImplementedException() : 2000;

        public int GetMaintenancePrice() => IsBought ? 500 : 0;

        public int GetPlayerAmount(float popularity) =>
            IsBought ? (int) Mathf.Clamp(_random.Next(1, 6) + popularity * 15, 1, 15) : 0;

        public float GetPopularityIncrease() => IsBought ? 0.2f : 0;

        public float GetPercentOfIncome() => 0.05f;
    }
}