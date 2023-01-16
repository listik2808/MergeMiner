using Source.Scripts.UI.OfflineReward;
using UnityEngine;

namespace Source.Scripts.UI
{
    public class MergeMenu : MenuBase
    {
        [SerializeField] private UIShovelInfo _shovelInfo;
        [SerializeField] private BuyShovelButton _buyShovelButton;
        [SerializeField] private DropShovelButton _dropShovelButton;
        [SerializeField] private AdvBoxButton _advBoxButton;
        [SerializeField] private OfflineRewardView _offlineRewardView;

        public UIShovelInfo ShovelInfo => _shovelInfo;
        public BuyShovelButton BuyShovelButton => _buyShovelButton;
        public DropShovelButton DropShovelButton => _dropShovelButton;
        public AdvBoxButton AdvBoxButton => _advBoxButton;
        public OfflineRewardView OfflineRewardView => _offlineRewardView;
    }
}
