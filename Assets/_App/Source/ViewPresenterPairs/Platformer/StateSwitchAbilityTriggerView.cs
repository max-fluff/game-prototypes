using System;
using System.Collections.Generic;

namespace MaxFluff.Prototypes
{
    public class StateSwitchAbilityTriggerView : ViewBase
    {
        public PlayerTrigger trigger;
        public bool isEnabling;
    }

    public class StateSwitchAbilityTriggerPresenter : PresenterBase<StateSwitchAbilityTriggerView>
    {
        public event Action<bool> OnSetStateSwitchAbilityState;

        public StateSwitchAbilityTriggerPresenter(StateSwitchAbilityTriggerView view) : base(view)
        {
            view.trigger.OnPlayerEnter += SendOnSetStateSwitchAbilityState;
        }

        private void SendOnSetStateSwitchAbilityState() =>
            OnSetStateSwitchAbilityState?.Invoke(_view.isEnabling);
    }
}