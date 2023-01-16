using System.Collections.Generic;
using System.Linq;
using Source.Scripts.StaticData;
using Unity.VisualScripting;
using UnityEngine;

namespace Source.Scripts.Infrastructure.Services.StaticData
{
    public class StaticDataService : IStaticDataService
    {
        private const string GameDataPath = "StaticData/GameData/GameConfig";
        private const string ShovelInfoDataPath = "StaticData/CellContentData/CellContentInfo";
        private const string RewardForShovelDataPath = "StaticData/RewardsData/RewardForShovelConfig";
        
        private Dictionary<int, CellContentConfig> _cellContentConfigs;
        private CellContentInfo _cellContentInfo;
        private Reward[] _rewardConfigs;
        private GameConfig _gameConfig;
        

        public void LoadGameConfig() => 
            _gameConfig = Resources.Load<GameConfig>(GameDataPath);

        public void LoadCellContentInfo()
        {
            _cellContentInfo = Resources
                .Load<CellContentInfo>(ShovelInfoDataPath);

            _cellContentConfigs = _cellContentInfo.ShovelConfigs
                .ToDictionary(x => x.CellContentPrefab.ID, x => x);

            var giftConfigs = _cellContentInfo.GiftConfigs
                .ToDictionary(x => x.CellContentPrefab.ID, x => x);

            var advGiftConfigs = _cellContentInfo.AdvGiftConfigs
                .ToDictionary(x => x.CellContentPrefab.ID, x => x);
            
            _cellContentConfigs.AddRange(giftConfigs);
            _cellContentConfigs.AddRange(advGiftConfigs);
        }

        public void LoadRewardForShovelConfigs() =>
            _rewardConfigs = Resources
                .Load<RewardForShovelConfig>(RewardForShovelDataPath).RewardForLevels;

        public LevelConfig ForLevel(int levelIndex) =>
            levelIndex >= 0 && levelIndex < _gameConfig.LevelConfigs.Length
                ? _gameConfig.LevelConfigs[levelIndex]
                : null;

        public CellContentInfo ForShovelInfo() =>
            _cellContentInfo;

        public CellContentConfig ForCellContent(int id) =>
            _cellContentConfigs.TryGetValue(id, out CellContentConfig config)
                ? config
                : null;

        public Reward ForShovelReward(int id)
        {
            id = Mathf.Clamp(id, 0, _rewardConfigs.Length - 1);
            return _rewardConfigs[id];
        }

        public int ForRepeatGameFromLevelNumber() =>
            _gameConfig.RepeatGameFromLevel;
    }
}
