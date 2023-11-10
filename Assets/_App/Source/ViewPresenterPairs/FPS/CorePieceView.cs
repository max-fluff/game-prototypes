using System;
using UnityEngine;

namespace MaxFluff.Prototypes.FPS
{
    public class CorePieceView : ViewBase, IZappableObject
    {
        public event Action OnZapped;
        public Material DisabledMaterial;
        public Renderer Renderer;

        public void Zap()
        {
            OnZapped?.Invoke();
        }
    }

    public class CorePiecePresenter : PresenterBase<CorePieceView>
    {
        public event Action OnZapped;

        private bool _isZapped;

        public CorePiecePresenter(CorePieceView view) : base(view)
        {
            _view.OnZapped += SendOnZapped;
        }

        private void SendOnZapped()
        {
            if (_isZapped) return;
            _isZapped = true;
            OnZapped?.Invoke();
            _view.Renderer.material = _view.DisabledMaterial;
        }
    }
}