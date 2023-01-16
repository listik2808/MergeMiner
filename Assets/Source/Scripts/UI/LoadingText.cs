using TMPro;
using UnityEngine;

namespace Source.Scripts.UI
{
    public class LoadingText : MonoBehaviour
    {
        private const string RussianTranslationCode = "ru";
        private const string EnglishTranslationCode = "en";
        private const string TurkishTranslationCode = "tr";

        [SerializeField] private TMP_Text _loadingText;
        [SerializeField] private string[] _translations;

        public void SetText(string language)
        {
            _loadingText.text = language switch
            {
                RussianTranslationCode => _translations[0],
                EnglishTranslationCode => _translations[1],
                TurkishTranslationCode => _translations[2],
                _ => _translations[1]
            };
        }
    }
}
