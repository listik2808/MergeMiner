using System.Collections.Generic;
using DG.Tweening;
using Source.Scripts.Infrastructure.Services.StaticData;
using Source.Scripts.UI.RewardMultiplierBar;
using UnityEngine;
using UnityEngine.UI;

namespace Source.Scripts.UI
{
    public class LevelCompleteMenu : MenuBase
    {
        [SerializeField] private Header header;
        [SerializeField] private Button _nextButton;
        [SerializeField] private RewardMultiplierButton _rewardButton;
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private CollectedRewardContainer _collectedRewardContainer;
        [SerializeField] [Min(0)] private float _appearDelay = 1f;
        [SerializeField] [Min(0)] private float _appearDuration = 0.5f;

        public Header Header => header;
        public Button NextButton => _nextButton;
        public RewardMultiplierButton RewardButton => _rewardButton;
        
        private void Awake() => Hide();

        public void Initialize(IStaticDataService staticData) =>
            _collectedRewardContainer.Initialize(staticData);

        public override void Show()
        {
            base.Show();
            _canvasGroup.alpha = 0;

            Sequence sequense = DOTween.Sequence();

            sequense.Append(_canvasGroup.DOFade(1, _appearDuration)).PrependInterval(_appearDelay);
        }

        public void Show(IReadOnlyList<int> rewardIds)
        {
            Show();
            foreach (var id in rewardIds)
                _collectedRewardContainer.ShowReward(id);
        }
    }
}
