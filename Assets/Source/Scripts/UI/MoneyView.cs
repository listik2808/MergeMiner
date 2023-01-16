using TMPro;
using UnityEngine;

namespace Source.Scripts.UI
{
    public class MoneyView : MonoBehaviour
    {
        [SerializeField] private TMP_Text _text;

        public void SetText(int value)
        {
            _text.text = value.ToString();

            if (value > 1000000000)
            {
                float textValue = (float)value / 1000000000;
                _text.text = textValue.ToString("0.00") + "M";
            }
            if (value > 1000000)
            {
                float textValue = (float)value / 1000000;
                _text.text = textValue.ToString("0.0") + "KK";
            }
            else if (value > 1000)
            {
                float textValue = (float)value / 1000;
                _text.text = textValue.ToString("0.0") + "K";
            }
        }
    }
}
