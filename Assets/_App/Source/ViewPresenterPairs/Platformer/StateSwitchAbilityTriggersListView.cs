using System;

namespace MaxFluff.Prototypes
{
    public class StateSwitchAbilityTriggersListView : ViewBase
    {
    }

    public class StateSwitchAbilityTriggersListPresenter : PresenterBase<StateSwitchAbilityTriggersListView>
    {
        public event Action<bool> OnSetStateSwitchAbilityState;

        public StateSwitchAbilityTriggersListPresenter(StateSwitchAbilityTriggersListView view) : base(view)
        {
            foreach (var trigger in view.GetComponentsInChildren<StateSwitchAbilityTriggerView>())
            {
                var triggerPresenter = new StateSwitchAbilityTriggerPresenter(trigger);
                triggerPresenter.OnSetStateSwitchAbilityState += SendOnSetStateSwitchAbilityState;
            }
        }

        private void SendOnSetStateSwitchAbilityState(bool isEnabling) =>
            OnSetStateSwitchAbilityState?.Invoke(isEnabling);
    }
}