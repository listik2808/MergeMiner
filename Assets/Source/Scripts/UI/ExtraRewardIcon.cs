using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Source.Scripts.UI
{
    public class ExtraRewardIcon : MonoBehaviour
    {
        [SerializeField] private Image _icon;
        [SerializeField] private TMP_Text _shovelGradeText;

        private void Awake() =>
            _shovelGradeText.text = "";

        public void SetIcon(Sprite icon) =>
            _icon.sprite = icon;

        public void SetShovelGradeText(int grade) =>
            _shovelGradeText.text = grade.ToString();
    }
}
