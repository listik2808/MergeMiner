using Source.Scripts.Infrastructure.Services.StaticData;
using Source.Scripts.Logic.GoalChecker;
using Source.Scripts.MergedObjects;
using Source.Scripts.SaveSystem;
using Source.Scripts.StaticData;
using Source.Scripts.UI.Tutorial;
using UnityEngine;
using Grid = Source.Scripts.Logic.GridBehaviour.Grid;

namespace Source.Scripts.UI
{
    public class UI : MonoBehaviour
    {
        [SerializeField] private PlayMenu _playMenu;
        [SerializeField] private MergeMenu _mergeMenu;
        [SerializeField] private LevelCompleteMenu _levelCompleteMenu;
        [SerializeField] private RewardMenu _rewardMenu;
        [SerializeField] private GoalPreviewMenu _goalPreviewMenu;
        [SerializeField] private TutorialCanvas _tutorialCanvas;
        [SerializeField] private OfflineRewardMenu _offlineRewardMenu;
        [SerializeField] private AudioButton _audioButton;

        private Grid _grid;
        private IStorage _storage;
        private IStaticDataService _staticData;
        private GoalBase _goal;

        public Grid Grid => _grid;
        public PlayMenu PlayMenu => _playMenu;
        public MergeMenu MergeMenu => _mergeMenu;
        public RewardMenu RewardMenu => _rewardMenu;
        public LevelCompleteMenu LevelCompleteMenu => _levelCompleteMenu;
        public GoalPreviewMenu GoalPreviewMenu => _goalPreviewMenu;
        public TutorialCanvas TutorialCanvas => _tutorialCanvas;
        public OfflineRewardMenu OfflineRewardMenu => _offlineRewardMenu;
        public IStorage Storage => _storage;
        public AudioButton AudioButton => _audioButton;
        public int CoinsReward { get; private set; }

        public void Construct(Grid grid, IStorage storage, IStaticDataService staticData, GoalBase goal)
        {
            _grid = grid;
            _storage = storage;
            _staticData = staticData;
            _offlineRewardMenu.Initialize(_staticData, _storage);
            _rewardMenu.Initialize(_staticData);
            _levelCompleteMenu.Initialize(_staticData);
            _playMenu.SetLevelNumberText(_storage.GetDisplayedLevelNumber());
            _goal = goal;
            SetGoal();
            _storage.Changed += OnChanged;
            OnChanged();
            CoinsReward = _staticData.ForLevel(_storage.GetLevel()).LevelReward;
        }

        private void OnDestroy()
        {
            _storage.Changed -= OnChanged;
            if (_goal != null)
                _goal.Checked += OnGoalChecked;
        }

        private void OnChanged()
        {
            CellContentInfo cellContentInfo = _staticData.ForShovelInfo();
            CellContentConfig cellContentConfig = _staticData.ForCellContent(_storage.GetInt(DataNames.CurrentShovelLevel));
            
            int soft = _storage.GetSoft();
            int price = cellContentConfig.CellContentPrefab.GetComponent<MergebleObject>().Price;

            _playMenu.SoftPanel.SetSoftText(soft);
            _mergeMenu.BuyShovelButton.SetShovelPriceText(price);
            _mergeMenu.BuyShovelButton.SetShovelIcon(cellContentConfig.CellContentIcon);
            _mergeMenu.BuyShovelButton.SetShovelGradeText(_storage.GetInt(DataNames.CurrentShovelLevel));
            _mergeMenu.ShovelInfo.Show(
                _storage.GetInt(DataNames.PurchasedShovelsNumber),
                _storage.GetInt(DataNames.CurrentShovelLevel) < cellContentInfo.ShovelCountsToNextPurchaseLevel.Length
                    ? cellContentInfo.ShovelCountsToNextPurchaseLevel[_storage.GetInt(DataNames.CurrentShovelLevel)]
                    : cellContentInfo.ShovelCountsToNextPurchaseLevel[_storage.GetInt(DataNames.CurrentShovelLevel) - 1]);

            if (soft < price)
            {
                _mergeMenu.BuyShovelButton.CanvasGroup.interactable = false;
                _mergeMenu.BuyShovelButton.CanvasGroup.alpha = 0.5f;
            }
            else
            {
                _mergeMenu.BuyShovelButton.CanvasGroup.interactable = true;
                _mergeMenu.BuyShovelButton.CanvasGroup.alpha = 1.0f;
            }
        }

        private void SetGoal()
        {
            _playMenu.GoalPanel.SetIcon(_goal.GoalIcon);
            _goal.Checked += OnGoalChecked;

            if (_goal is ChestGoal chestGoal)
                _playMenu.GoalPanel.SetCount(chestGoal.TargetCount);
        }

        private void OnGoalChecked(GoalBase goal)
        {
            if (goal is ChestGoal chestGoal)
            {
                _playMenu.GoalPanel.SetCount(chestGoal.TargetCount - chestGoal.CurrentOpenedChestCount);
                _playMenu.GoalPanel.SetCheckIcon(chestGoal.TargetCount <= chestGoal.CurrentOpenedChestCount);
            }

            if (goal is BossGoal bossGoal)
            {
                _playMenu.GoalPanel.SetCheckIcon(bossGoal.IsBossDefeated);
            }
        }
    }
}
