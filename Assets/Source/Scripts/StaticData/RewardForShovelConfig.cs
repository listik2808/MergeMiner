using System;
using UnityEngine;

namespace Source.Scripts.StaticData
{
    [CreateAssetMenu(fileName = "New_RewardForShovelConfig", menuName = "Static Data/RewardForShovelConfig")]
    public class RewardForShovelConfig : ScriptableObject
    {
        public Reward[] RewardForLevels;
    }

    [Serializable]
    public class Reward
    {
        public int[] СellContentIds;
    }
}