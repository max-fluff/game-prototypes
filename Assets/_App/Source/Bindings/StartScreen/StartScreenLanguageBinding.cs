using System.Linq;

namespace Omega.Kulibin
{
    public sealed class StartScreenLanguageBinding : IInitBinding
    {
        private readonly StartScreenPresenter _startScreen;
        private readonly LocalizationService _localization;

        public StartScreenLanguageBinding(
            StartScreenPresenter startScreen,
            LocalizationService localization
            )
        {
            _startScreen = startScreen;
            _localization = localization;
        }

        public void Init()
        {
            _startScreen.OnLanguageClicked += ChangeLanguage;
            
            _startScreen.SetLanguageCode(_localization.CurrentLanguageCode.ToUpper());
        }

        private void ChangeLanguage()
        {
            var currentLanguage = _localization.CurrentLanguage;
            var allLanguages = _localization.AllLanguage;

            var listAllLanguage = allLanguages.ToList();
            var indexNextLanguage = listAllLanguage.IndexOf(currentLanguage) + 1;

            currentLanguage = listAllLanguage[indexNextLanguage % listAllLanguage.Count];
            
            _localization.SetLanguage(currentLanguage);
            _startScreen.SetLanguageCode(_localization.CurrentLanguageCode.ToUpper());
        }
    }
}