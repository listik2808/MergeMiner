using DG.Tweening;
using TMPro;
using UnityEngine;

namespace Source.Scripts.UI
{
    public class PlayMenu : MenuBase
    {
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private GoalPanel _goalPanel;
        [SerializeField] private SoftPanel _softPanel;
        [SerializeField] private TMP_Text _levelNumberText;
        [SerializeField] [Min(0)] private float _appearDuration = 0.5f;

        private Sequence _sequence;
        
        public GoalPanel GoalPanel => _goalPanel;
        public SoftPanel SoftPanel => _softPanel;

        public override void Show()
        {
            base.Show();
            _canvasGroup.alpha = 0;
            _sequence = DOTween.Sequence();
            _sequence.Append(_canvasGroup.DOFade(1, _appearDuration));
        }

        public void SetLevelNumberText(int levelNumber) =>
            _levelNumberText.text = levelNumber.ToString();
    }
}
