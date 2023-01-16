using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Source.Scripts.UI
{
    public class BuyShovelButton : MonoBehaviour
    {
        [SerializeField] private Button _button;
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private MoneyView _shovelPriceText;
        [SerializeField] private Image _shovelIcon;
        [SerializeField] private TMP_Text _shovelGradeText;
        [SerializeField] private Transform _handPointerPosition;

        public Button Button => _button;
        public Transform HandPointerPosition => _handPointerPosition;
        public CanvasGroup CanvasGroup => _canvasGroup;

        public void SetShovelPriceText(int price) =>
            _shovelPriceText.SetText(price);

        public void SetShovelIcon(Sprite icon) =>
            _shovelIcon.sprite = icon;

        public void SetShovelGradeText(int grade) =>
            _shovelGradeText.text = grade.ToString();
    }
}
