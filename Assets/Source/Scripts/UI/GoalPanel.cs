using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Source.Scripts.UI
{
    public class GoalPanel : MonoBehaviour
    {
        [SerializeField] private Image _goalIcon;
        [SerializeField] private Image _checkIcon;
        [SerializeField] private TMP_Text _countText;

        private void Awake()
        {
            SetCheckIcon(false);
        }

        public void SetIcon(Sprite icon) =>
            _goalIcon.sprite = icon;

        public void SetCount(int value)
        {
            if (value <= 0)
            {
                _countText.text = "";
                return;
            }
            _countText.text = value.ToString();
        }

        public void SetCheckIcon(bool isEnable)
        {
            _checkIcon.enabled = isEnable;
        }
    }
}
