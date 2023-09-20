using System;
using Lean.Gui;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Omega.Kulibin
{
    public sealed class StartScreenView : ViewBase
    {
        [Header("Other")] 
        public LeanButton File;
        public LeanButton Site;
        public TextMeshProUGUI Email;
        public LeanButton LogOut;
        public LeanButton Authorize;
        public Canvas MainCanvas;
        public LeanButton Language;
        public TextMeshProUGUI LanguageCode;
        public LeanButton CommunityTelegram;
        public LeanButton UpdatesTelegram;
    }

    public sealed class StartScreenPresenter : PresenterBase<StartScreenView>
    {
        public event Action OnQuitClicked;
        public event Action OnAuthorizeClicked;
        public event Action OnFileClicked;
        public event Action OnSiteClicked;
        public event Action OnCommunityTelegramClicked;
        public event Action OnUpdatesTelegramClicked;
        public event Action OnLanguageClicked;
        
        public StartScreenPresenter(StartScreenView view) : base(view)
        {
            _view.LogOut.OnClick.AddListener(QuitClick);
            _view.Authorize.OnClick.AddListener(AuthorizeClick);
            _view.File.OnClick.AddListener(FileClick);
            _view.Site.OnClick.AddListener(SiteClick);
            _view.Language.OnClick.AddListener(LanguageClick);
            _view.CommunityTelegram.OnClick.AddListener(CommunityTelegramClick);
            _view.UpdatesTelegram.OnClick.AddListener(UpdatesTelegramClick);
        }

        private void QuitClick() => OnQuitClicked?.Invoke();
        private void AuthorizeClick() => OnAuthorizeClicked?.Invoke();
        private void FileClick() => OnFileClicked?.Invoke();
        private void SiteClick() => OnSiteClicked?.Invoke();
        private void CommunityTelegramClick() => OnCommunityTelegramClicked?.Invoke();
        private void UpdatesTelegramClick() => OnUpdatesTelegramClicked?.Invoke();
        private void LanguageClick() => OnLanguageClicked?.Invoke();
        public void SetCurrentEmail(string email) => _view.Email.SetText(email);

        public void SetAuthorizedState(bool isProperlyAuthorized)
        {
            _view.LogOut.gameObject.SetActive(isProperlyAuthorized);
            _view.Authorize.gameObject.SetActive(!isProperlyAuthorized);
        }

        public void DisableAuthorizationFeatures()
        {
            _view.LogOut.gameObject.SetActive(false);
            _view.Authorize.gameObject.SetActive(false);
        }
        
        public void SetLanguageCode(string code) => _view.LanguageCode.text = code;
        
        public void SetCanvasScale(float newScale) =>
            _view.MainCanvas.GetComponent<CanvasScaler>().scaleFactor = newScale;
    }
}