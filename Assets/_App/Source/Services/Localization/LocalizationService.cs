using System.Collections.Generic;
using I2.Loc;

namespace MaxFluff.Prototypes
{
    public class LocalizationService
    {
        public void SetLanguage(string language)
        {
            if (LocalizationManager.HasLanguage(language))
                LocalizationManager.CurrentLanguage = language;
        }

        public string CurrentLanguage => LocalizationManager.CurrentLanguage;
        public string CurrentLanguageCode => LocalizationManager.CurrentLanguageCode;
        public IEnumerable<string> AllLanguage => LocalizationManager.GetAllLanguages();
    }
}