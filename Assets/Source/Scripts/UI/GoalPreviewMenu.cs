using DG.Tweening;
using Source.Scripts.Logic.GoalChecker;
using UnityEngine;

namespace Source.Scripts.UI
{
    public class GoalPreviewMenu : MenuBase
    {
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private ChestGoalScreen _chestGoalScreen;
        [SerializeField] private BossGoalScreen _bossGoalScreen;
        [SerializeField] [Min(0)] private float _appearDuration = 1.0f;
        [SerializeField] [Min(0)] private float _disappearDuration = 1.0f;
        
        private Sequence _sequence;
        
        public void ShowFor(GoalBase goal, int levelNumber)
        {
            switch (goal)
            {
                case BossGoal:
                    _bossGoalScreen.Show();
                    Appear();
                    break;
                case ChestGoal chestGoal:
                    _chestGoalScreen.ShowWith(chestGoal.TargetCount, levelNumber);
                    Appear();
                    break;
            }
        }

        public override void Hide()
        {
            _chestGoalScreen.Hide();
            _bossGoalScreen.Hide();
            _sequence = DOTween.Sequence();
            _sequence.Append(_canvasGroup.DOFade(0, _disappearDuration)).OnComplete(base.Hide);
        }

        private void Appear()
        {
            base.Show();
            _canvasGroup.alpha = 0;
            _sequence = DOTween.Sequence();
            _sequence.Append(_canvasGroup.DOFade(1, _appearDuration));
        }
    }
}