using UnityEngine;

namespace Localization
{
    public class BillboardTextureSptiteChanger : MonoBehaviour
    {
        [SerializeField] private LanguageDetector languageDetector;
        [SerializeField] private Material _material;
        [SerializeField] private Texture _textureRus;
        [SerializeField] private Texture _textureEng;
        [SerializeField] private Texture _textureTr;

        private const string RussianTranslationCode = "ru";
        private const string EnglishTranslationCode = "en";
        private const string TurkishTranslationCode = "tr";

        public void OnLanguageDetected(string currentLanguage)
        {
            switch(currentLanguage)
            {
                case RussianTranslationCode:
                    Change(_textureRus);
                    break;
                case EnglishTranslationCode:
                    Change(_textureEng);
                    break;
                case TurkishTranslationCode:
                    Change(_textureTr);
                    break;
            }
        }

        private void Change(Texture texture)
        {
            _material.mainTexture = texture;
        }
    }
}
