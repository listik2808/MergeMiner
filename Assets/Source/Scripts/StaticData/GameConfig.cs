using UnityEngine;

namespace Source.Scripts.StaticData
{
    [CreateAssetMenu(fileName = "New_GameConfig", menuName = "Static Data/GameConfig")]
    public class GameConfig : ScriptableObject
    {
        [Min(1)]
        public int RepeatGameFromLevel = 1;
        public LevelConfig[] LevelConfigs;
    }

    [System.Serializable]
    public class LevelConfig
    {
        public string LevelElementsPath;
        public string SceneName;
        public string GoalPath;
        public Material Skybox;
        
        [Min(10)]
        public int CellCount = 10;
        
        [Min(10)]
        public int LevelReward = 10;

        public int[] ChestReward;
    }
}