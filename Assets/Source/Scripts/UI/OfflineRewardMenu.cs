using System.Collections.Generic;
using DG.Tweening;
using Source.Scripts.Infrastructure.Services.StaticData;
using Source.Scripts.MergedObjects;
using Source.Scripts.SaveSystem;
using Source.Scripts.StaticData;
using UnityEngine;
using UnityEngine.UI;

namespace Source.Scripts.UI
{
    public class OfflineRewardMenu : MenuBase
    {
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private ExtraRewardIcon _extraRewardIconPrefab;
        [SerializeField] private Transform _content;
        [SerializeField] private Button _rewardButton;
        [SerializeField] private Button _nextButton;
        [SerializeField] [Min(0)] private float _hideDuration = 0.05f;
        
        private IStaticDataService _staticData;
        private IStorage _storage;
        private Sequence _sequence;

        private List<ExtraRewardIcon> _icons = new List<ExtraRewardIcon>();

        public Button RewardButton => _rewardButton;
        public Button NextButton => _nextButton;

        private void Awake() =>
            base.Hide();

        public void Initialize(IStaticDataService staticData, IStorage storage)
        {
            _staticData = staticData;
            _storage = storage;
        }

        public override void Show()
        {
            base.Show();
            _canvasGroup.alpha = 1;
        }

        public override void Hide()
        {
            _sequence = DOTween.Sequence();
            _sequence.Append(_canvasGroup.DOFade(0, _hideDuration)).OnComplete(base.Hide);
        }

        public void CreateRewardView(int id)
        {
            if (id < 1)
                id = 1;
            
            ExtraRewardIcon newIcon = Instantiate(_extraRewardIconPrefab, _content);
            _icons.Add(newIcon);
            CellContentConfig config = _staticData.ForCellContent(id);
            if (config == null)
                id = _storage.GetInt(DataNames.MaxMergedShovelLevel);

            newIcon.SetIcon(_staticData.ForCellContent(id).CellContentIcon);
            if(_staticData.ForCellContent(id).CellContentPrefab is MergebleObject)
                newIcon.SetShovelGradeText(id);
        }

        public void ClearRewardView()
        {
            foreach (ExtraRewardIcon icon in _icons)
                Destroy(icon.gameObject);
            
            _icons.Clear();
        }
    }
}
