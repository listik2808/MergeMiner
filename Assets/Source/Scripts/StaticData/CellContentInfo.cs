using System;
using Source.Scripts.Logic.Cell;
using UnityEngine;

namespace Source.Scripts.StaticData
{
    [CreateAssetMenu(fileName = "New_CellContentInfo", menuName = "Static Data/CellContentInfo")]
    public class CellContentInfo : ScriptableObject
    {
        public int[] ShovelCountsToNextPurchaseLevel;

        public CellContentConfig[] ShovelConfigs;
        public CellContentConfig[] GiftConfigs;
        public CellContentConfig[] AdvGiftConfigs;
    }

    [Serializable]
    public class CellContentConfig
    {
        public Sprite CellContentIcon;
        public CellContent CellContentPrefab;
    }
}
