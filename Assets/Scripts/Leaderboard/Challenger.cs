using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Source.UI.Leaderboards
{
    public class Challenger : MonoBehaviour
    {
        [SerializeField] private string _name;
        [SerializeField] private int _coinCount;
        [SerializeField] private Sprite _rankIcon;
        [SerializeField] private int _rank;

        public string Name => _name;
        public int CoinCount => _coinCount;
        public Sprite RankIcon => _rankIcon;
        public int Rank => _rank;
    }
}
