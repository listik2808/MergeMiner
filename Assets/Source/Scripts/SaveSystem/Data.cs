using System;

namespace Source.Scripts.SaveSystem
{
    [Serializable]
    public class Data
    {
        public int LevelNumber = 1;
        public int DisplayedLevelNumber = 1;
        public int SessionCount = 0;
        public int Soft = 0;
        public string SaveTime = DateTime.MinValue.ToString();
        public string RegistrationDate = DateTime.Now.ToString();
        public string LastLoginDate = DateTime.Now.ToString();
        public string TimeSinceLastLogin = DateTime.MinValue.ToString();
        public string LastRewardTakingTime = DateTime.Now.ToString();
        public SerializedDictionary<string, float> Floats = new();
        public SerializedDictionary<string, int> Ints = new();
        public SerializedDictionary<string, string> Strings = new();
        public SerializedDictionary<string, Vector3Data> Vectors = new();
        public SerializedDictionary<string, QuaternionData> Quaternions = new();

        public Data()
        {
            Ints.Add(DataNames.CellContent + 2, 1);
            Ints.Add(DataNames.PurchasedShovelsNumber, 0);
            Ints.Add(DataNames.CurrentShovelLevel, 1);
            Ints.Add(DataNames.MaxMergedShovelLevel, 1);
            Ints.Add(DataNames.IsMusicPlaying, 1);
            Ints.Add(DataNames.IsTutorialComplete, 0);
            Ints.Add(DataNames.VideoAdShownCount, 0);
            Ints.Add(DataNames.IsAdShowing, 0);
        }
    }
}