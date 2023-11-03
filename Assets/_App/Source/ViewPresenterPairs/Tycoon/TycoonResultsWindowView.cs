using TMPro;
using UnityEngine;

namespace MaxFluff.Prototypes
{
    public class TycoonResultsWindowView : WindowViewBase
    {
        public GameObject RaidMoney;
        public GameObject Raided;
        public GameObject NotRaided;

        public TextMeshProUGUI Income;
        public TextMeshProUGUI Tax;
        public TextMeshProUGUI Service;
        public TextMeshProUGUI RaidLost;
        public TextMeshProUGUI Result;
    }

    public class TycoonResultsWindowPresenter : WindowPresenterBase<TycoonResultsWindowView>
    {
        public TycoonResultsWindowPresenter(TycoonResultsWindowView view) : base(view)
        {
        }

        public void Open(int income, int tax, int service, int raidLost, int result)
        {
            _view.RaidMoney.SetActive(raidLost > 0);
            _view.Raided.SetActive(raidLost > 0);
            _view.NotRaided.SetActive(raidLost == 0);

            _view.Income.SetText(income + "$");
            _view.Tax.SetText(tax + "$");
            _view.Service.SetText(service + "$");
            _view.Result.SetText(result + "$");
            _view.RaidLost.SetText(raidLost + "$");

            Open();
        }
    }
}