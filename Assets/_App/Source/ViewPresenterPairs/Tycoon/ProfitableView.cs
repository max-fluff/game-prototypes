namespace MaxFluff.Prototypes
{
    public abstract class ProfitableView: ViewBase
    {
        
    };
    public interface IProfitablePresenter
    {
        public int GetSellPrice();
        public int GetBuyPrice();
        public int GetMaintenancePrice();
        public int GetPlayerAmount(float popularity);
        public float GetPopularityIncrease();

        public float GetPercentOfIncome();
    }
}