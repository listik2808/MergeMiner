using Source.Scripts.Infrastructure.Services.StaticData;
using Source.Scripts.MergedObjects;
using UnityEngine;

namespace Source.Scripts.UI
{
    public class CollectedRewardContainer : MonoBehaviour
    {
        [SerializeField] private ExtraRewardIcon _extraRewardIconPrefab;

        private IStaticDataService _staticData;

        public void Initialize(IStaticDataService staticData) =>
            _staticData = staticData;
        
        public void ShowReward(int id)
        {
            ExtraRewardIcon newIcon = Instantiate(_extraRewardIconPrefab, transform);
            newIcon.SetIcon(_staticData.ForCellContent(id).CellContentIcon);
            if (_staticData.ForCellContent(id).CellContentPrefab is MergebleObject)
                newIcon.SetShovelGradeText(id);
        }
    }
}
