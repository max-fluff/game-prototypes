using System;
using UnityEngine;

namespace MaxFluff.Prototypes
{
    public class LevelElementView : ViewBase
    {
        public Transform PreviousConnectionJoint;
        public Transform NextConnectionJoint;
        public PlayerTrigger PlayerTrigger;
    }

    public class LevelElementPresenter : PresenterBase<LevelElementView>
    {
        public event Action OnPlayerTriggered;

        public LevelElementPresenter(LevelElementView view) : base(view)
        {
            view.PlayerTrigger.OnPlayerEnter.AddListener(SendOnPlayerTriggered);
        }

        public Vector3 GetPreviousConnectionPoint() => _view.PreviousConnectionJoint.position;
        public Vector3 GetNextConnectionPoint() => _view.NextConnectionJoint.position;
        public Vector3 GetPosition() => _view.transform.position;

        public void Move(Vector3 delta) => _view.transform.position += delta;
        public void RotateAroundW(float delta) => _view.transform.Rotate(_view.transform.right, delta * 90f);

        private void SendOnPlayerTriggered() => OnPlayerTriggered?.Invoke();

        public override void Destroy()
        {
            OnPlayerTriggered = null;
            base.Destroy();
        }
    }
}