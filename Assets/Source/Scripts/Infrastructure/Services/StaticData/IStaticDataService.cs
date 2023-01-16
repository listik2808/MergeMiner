using Source.Scripts.StaticData;

namespace Source.Scripts.Infrastructure.Services.StaticData
{
    public interface IStaticDataService : IService
    {
        void LoadGameConfig();
        LevelConfig ForLevel(int levelIndex);
        void LoadCellContentInfo();
        CellContentInfo ForShovelInfo();
        CellContentConfig ForCellContent(int id);
        void LoadRewardForShovelConfigs();
        Reward ForShovelReward(int id);
        int ForRepeatGameFromLevelNumber();
    }
}