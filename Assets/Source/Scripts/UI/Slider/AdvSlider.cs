using UnityEngine;
using UnityEngine.UI;

public class AdvSlider : MonoBehaviour
{
    [SerializeField] private Image _slider;

    public void SetValue(float value)
    {
        _slider.fillAmount = value;
    }
}