using System;
using Agava.YandexGames;
using Lean.Localization;
using TMPro;
using UnityEngine;

namespace Localization
{
    public class LanguageDetector : MonoBehaviour
    {
        [SerializeField] private LeanLocalization _leanLocalization;

        private string _language;
        
        private const string RussianTranslationCode = "ru";
        private const string EnglishTranslationCode = "en";
        private const string TurkishTranslationCode = "tr";
        private const string English = "English";
        private const string Russian = "Russian";
        private const string Turkish = "Turkish";

        public string RusLang => RussianTranslationCode;

        //public event Action<string> LanguageDetected;

        private void Start()
        {
#if !UNITY_EDITOR && UNITY_WEBGL
            _language = GetLanguage();

            switch (_language)
            {
                case RussianTranslationCode:
                    _leanLocalization.SetCurrentLanguage(Russian);
                    break;
                case EnglishTranslationCode:
                    _leanLocalization.SetCurrentLanguage(English);
                    break;
                case TurkishTranslationCode:
                    _leanLocalization.SetCurrentLanguage(Turkish);
                    break;
                default:
                    _leanLocalization.SetCurrentLanguage(Russian);
                    break;
            }
#endif
        }

        private string GetLanguage()
        {
            return YandexGamesSdk.Environment.i18n.lang;
        }
    }
}
