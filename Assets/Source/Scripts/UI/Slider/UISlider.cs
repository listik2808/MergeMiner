using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Source.Scripts.UI.Slider
{
    public class UISlider : MonoBehaviour
    {
        [SerializeField] private Image _filler;
        [SerializeField] private TMP_Text _text;

        public void SetValue(int currentValue, int maxValue)
        {
            _filler.fillAmount = (float) currentValue / maxValue;

            _text.text = (_filler.fillAmount * 100).ToString("0") + "%";
        }
    }
}
