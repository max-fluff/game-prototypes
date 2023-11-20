using System;
using Object = UnityEngine.Object;

namespace MaxFluff.Prototypes
{
    public abstract class PresenterBase<T> where T : ViewBase
    {
        protected readonly T _view;

        public event Action OnDestroy;

        protected PresenterBase(T view)
        {
            _view = view;
            _view.OnDestroying += SendOnDestroy;
        }

        private void SendOnDestroy() => OnDestroy?.Invoke();

        public virtual void Destroy()
        {
            Object.Destroy(_view.gameObject);
        }
    }
}