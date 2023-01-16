using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Source.Scripts.UI
{
    public class UIShovelInfo : MonoBehaviour
    {
        [SerializeField] private Image _filler;
        [SerializeField] private TMP_Text _purchasedShovelsNumberText;
        [SerializeField] private TMP_Text _shovelsCountToNextLevelText;

        public void Show(int purchasedShovel, int shovelsToNextLevel)
        {
            _purchasedShovelsNumberText.text = purchasedShovel.ToString();
            _shovelsCountToNextLevelText.text = shovelsToNextLevel.ToString();

            _filler.fillAmount = (float) purchasedShovel / shovelsToNextLevel;
        }
    }
}
