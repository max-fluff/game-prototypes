using System;

namespace MaxFluff.Prototypes.FPS
{
    public class MobileRetranslatorPowerUpView : ViewBase
    {
        public PlayerTrigger PlayerTrigger;
    }

    public class MobileRetranslatorPowerUpPresenter : PresenterBase<MobileRetranslatorPowerUpView>
    {
        public event Action OnPowerUpCollected;

        public MobileRetranslatorPowerUpPresenter(MobileRetranslatorPowerUpView view) : base(view)
        {
            view.PlayerTrigger.OnPlayerEnter.AddListener(SendOnPowerUpCollected);
        }

        private void SendOnPowerUpCollected()
        {
            OnPowerUpCollected?.Invoke();
            _view.gameObject.SetActive(false);
        }
    }
}