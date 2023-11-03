using System.Collections.Generic;
using Lean.Gui;
using UnityEngine;
using UnityEngine.EventSystems;

namespace MaxFluff.Prototypes
{
    public class WindowsOrganizerView : ViewBase
    {
        [Header("Window Closer")] public LeanWindowCloser WindowCloser;

        [Header("Blocker")] public Transform Blocker;
        public LeanButton BlockerButton;
        public CanvasGroup BlockerCanvas;
    }

    public class WindowsOrganizerPresenter : PresenterBase<WindowsOrganizerView>
    {
        private readonly List<IWindowPresenter> _activeWindows = new List<IWindowPresenter>();
        private readonly List<IWindowPresenter> _registeredWindows = new List<IWindowPresenter>();

        public WindowsOrganizerPresenter(WindowsOrganizerView view) : base(view)
        {
            //view.BlockerButton.OnClick.AddListener(CloseByBlocker);

            CloseKey = view.WindowCloser.CloseKey;
            view.WindowCloser.CloseKey = KeyCode.None;
        }

        public KeyCode CloseKey { get; }

        public bool IsAnyWindowOpened { get; private set; }

        public bool MouseInputRequired
        {
            get
            {
                var isOutsideClickNeeded = false;
                for (var i = _activeWindows.Count - 1; i >= 0; i--)
                {
                    if (_activeWindows[i].ClosesOnOutsideClick &&
                        !_activeWindows[i].IgnoreOutsideClickClosing)
                    {
                        isOutsideClickNeeded = true;
                        break;
                    }

                    if (_activeWindows[i].NeedBlocker)
                        break;
                }

                return isOutsideClickNeeded;
            }
        }

        public bool KeyboardInputRequired => _activeWindows.Count > 0 &&
                                             TopWindow.MayBeClosed;

        private IWindowPresenter TopWindow => _activeWindows[_activeWindows.Count - 1];

        public void CacheOpenedWindowsCount()
        {
            IsAnyWindowOpened = _activeWindows.Count > 0;
        }

        public void ProcessCloseKey()
        {
            _view.WindowCloser.CloseTopMost();
        }

        public void ProcessMouseDown(List<RaycastResult> raycastResults)
        {
            if (raycastResults.Count == 0)
            {
                CloseTop();
                return;
            }

            var firstResult = raycastResults[0];
            if (firstResult.gameObject.GetComponentInParent<WindowsOrganizerView>() == null)
                CloseTop();
        }

        public void RegisterWindow(IWindowPresenter window)
        {
            if (_registeredWindows.Contains(window))
                return;

            window.IsOpeningAllowedDelegate = IsOpeningAllowed;

            window.OnOpened += MoveToTopClosure;
            window.OnClosed += PopWindowAndLowerBlockerClosure;

            _registeredWindows.Add(window);

            void MoveToTopClosure() =>
                MoveWindowToTop(window);

            void PopWindowAndLowerBlockerClosure() =>
                PopWindowAndLowerBlocker(window);
        }

        private bool IsOpeningAllowed()
        {
            return _activeWindows.Count == 0 ||
                   TopWindow.AllowAnotherWindowOnTop;
        }

        private void MoveWindowToTop(IWindowPresenter window)
        {
            if (window.NeedBlocker)
                ShowBlocker();

            window.Transform.SetAsLastSibling();

            _activeWindows.Add(window);
        }

        private void PopWindowAndLowerBlocker(IWindowPresenter window)
        {
            _activeWindows.Remove(window);
            LowerBlocker();
        }

        private void LowerBlocker()
        {
            var needToHideBlocker = true;
            foreach (var window in _activeWindows)
            {
                if (window.NeedBlocker)
                {
                    var windowIndex = window.Transform.GetSiblingIndex();
                    _view.Blocker.SetSiblingIndex(windowIndex);
                    needToHideBlocker = false;
                    break;
                }
            }

            if (needToHideBlocker)
                HideBlocker();
        }

        private void CloseByBlocker()
        {
            if (_activeWindows.Count == 0)
            {
                HideBlocker();
                return;
            }

            for (var i = _activeWindows.Count - 1; i >= 0; i--)
            {
                if (_activeWindows[i].NeedBlocker &&
                    _activeWindows[i].MayBeClosed)
                {
                    _activeWindows[i].Close();
                    return;
                }
            }
        }

        private void CloseTop()
        {
            if (_activeWindows.Count == 0 ||
                !TopWindow.MayBeClosed)
                return;

            _view.WindowCloser.CloseTopMost();
        }

        public void CloseAll()
        {
            _view.WindowCloser.CloseAll();
        }

        private void ShowBlocker()
        {
            _view.Blocker.SetAsLastSibling();
            _view.BlockerCanvas.alpha = 1f;
            _view.BlockerCanvas.blocksRaycasts = true;
        }

        private void HideBlocker()
        {
            _view.BlockerCanvas.alpha = 0f;
            _view.BlockerCanvas.blocksRaycasts = false;
        }
    }
}