using Agava.YandexGames;
using Lean.Localization;
using UnityEngine;

namespace Menu
{
    public class Localization : MonoBehaviour
    {
        private const string EnglishCode = "English";
        private const string RussianCode = "Russian";
        private const string TurkishCode = "Turkish";
        private const string English = "en";
        private const string Russian = "ru";
        private const string Turkish = "tr";

        [SerializeField] private LeanLocalization _localization;
#if UNITY_WEBGL && !UNITY_EDITOR
        private void Awake()
        {
            ChangeLanguage();
        }
        
        private void ChangeLanguage()
        {
            string languageCode = YandexGamesSdk.Environment.i18n.lang;

            switch (languageCode)
            {
                case English:
                    _localization.SetCurrentLanguage(EnglishCode);
                    break;
                
                case Russian:
                    _localization.SetCurrentLanguage(RussianCode);
                    break;
                
                case Turkish:
                    _localization.SetCurrentLanguage(TurkishCode);
                    break;
            }
        }
#endif
    }
}