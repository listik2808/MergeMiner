using Source.Scripts.UI.RewardMultiplierBar;
using UnityEngine;

namespace Source.Scripts.UI
{
    public class RewardDisplay : MonoBehaviour
    {
        [SerializeField] private MoneyView _moneyView;
        [SerializeField] private UI _uI;
        [SerializeField] private MultiplierCursor _cursor;

        private int _levelReward;
        private int _multipliedRewardValue;
    
        private void Awake()
        {
            _levelReward = _uI.CoinsReward;
        }

        private void OnEnable()
        {
            _cursor.MultiplierUpdated += OnMultiplierUpdated;
        }

        private void OnDisable()
        {
            _cursor.MultiplierUpdated -= OnMultiplierUpdated;
        }

        private void OnMultiplierUpdated(int multiplierValue)
        {
            _multipliedRewardValue = _uI.CoinsReward * multiplierValue;
            _moneyView.SetText(_multipliedRewardValue);
        }
    }
}
