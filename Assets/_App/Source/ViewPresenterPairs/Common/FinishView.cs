using System;
using System.Collections.Generic;
using UnityEngine;

namespace MaxFluff.Prototypes
{
    public class FinishView : ViewBase
    {
        public List<Trigger> Triggers;
    }

    public class FinishPresenter : PresenterBase<FinishView>
    {
        public event Action<Collider> OnTriggerEntered;

        public FinishPresenter(FinishView view) : base(view)
        {
            foreach (var trigger in _view.Triggers)
                trigger.OnTriggerEntered += OnTriggerEnter;
        }

        private void OnTriggerEnter(Collider other) => OnTriggerEntered?.Invoke(other);
    }
}