using Source.Scripts.Infrastructure.Services;
using Source.Scripts.Infrastructure.Services.StaticData;
using Source.Scripts.MergedObjects;
using UnityEngine;

namespace Source.Scripts.UI
{
    public class OfflineIncoming : MonoBehaviour
    {
        [SerializeField] private ExtraRewardIcon _extraRewardIconPrefab;
        [SerializeField] private Transform _content;
        [SerializeField] [Range(0, 4)] private int _offlineRewardsCount = 4;

        private IStaticDataService _staticData;

        private void Awake()
        {
            _staticData = AllServices.Container.Single<IStaticDataService>();
        }

        private void Start()
        {
            for (int i = 1; i <= _offlineRewardsCount; i++)
            {
                CreateRewardView(5);
            }
        }

        private void CreateRewardView(int id)
        {
            ExtraRewardIcon newIcon = Instantiate(_extraRewardIconPrefab, _content);
            newIcon.SetIcon(_staticData.ForCellContent(id).CellContentIcon);
            if(_staticData.ForCellContent(id).CellContentPrefab is MergebleObject)
                newIcon.SetShovelGradeText(id);
        }
    }
}
