using System;
using System.Collections.Generic;
using UnityEngine;

namespace MaxFluff.Prototypes
{
    public class BordersView : ViewBase
    {
        public List<Trigger> Triggers;
    }

    public class BordersPresenter : PresenterBase<BordersView>
    {
        public event Action<Collider> OnTriggerEntered;

        public BordersPresenter(BordersView view) : base(view)
        {
            foreach (var trigger in _view.Triggers)
                trigger.OnTriggerEntered += OnTriggerEnter;
        }

        private void OnTriggerEnter(Collider other) => OnTriggerEntered?.Invoke(other);
    }
}