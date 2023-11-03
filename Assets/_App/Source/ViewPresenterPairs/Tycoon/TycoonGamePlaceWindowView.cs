using Lean.Gui;

namespace MaxFluff.Prototypes
{
    public class TycoonGamePlaceWindowView : WindowViewBase
    {
        public LeanButton PokerButton;
        public LeanButton BlackJackButton;
        public LeanButton RouletteButton;
    }

    public class TycoonGamePlaceWindowPresenter : WindowPresenterBase<TycoonGamePlaceWindowView>
    {
        public StationState State = StationState.None;
        
        public override bool NeedBlocker => true;

        public TycoonGamePlaceWindowPresenter(TycoonGamePlaceWindowView view) : base(view)
        {
            view.PokerButton.OnClick.AddListener(BuyPoker);
            view.BlackJackButton.OnClick.AddListener(BuyBlackJack);
            view.RouletteButton.OnClick.AddListener(BuyRoulette);
        }

        private void BuyPoker()
        {
            State = StationState.Poker;

            Close();

            State = StationState.None;
        }

        private void BuyBlackJack()
        {
            State = StationState.BlackJack;

            Close();

            State = StationState.None;
        }

        private void BuyRoulette()
        {
            State = StationState.Roulette;

            Close();

            State = StationState.None;
        }
    }
}