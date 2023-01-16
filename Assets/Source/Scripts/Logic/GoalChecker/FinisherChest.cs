using Source.Animations.Chest;
using Source.Scripts.Infrastructure.Services;
using Source.Scripts.Infrastructure.Services.StaticData;
using Source.Scripts.Logic.GoalChecker.Boss;
using Source.Scripts.MergedObjects;
using UnityEngine;

namespace Source.Scripts.Logic.GoalChecker
{
    [SelectionBase]
    public class FinisherChest : MonoBehaviour
    {
        [SerializeField] private ParticleSystem[] particleSystems;
        [SerializeField] private FinisherChestAnimator _chestAnimator;
        [SerializeField] private Transform _rewardViewPlace;
        [SerializeField] [Min(0)] private int _rewardId = 1;
        [SerializeField] private bool _isBossChest;

        private IStaticDataService _staticData;
        
        public FinisherChestAnimator Animator => _chestAnimator;
        public int RewardId => _rewardId;
        public bool IsHit { get; private set; }

        private void Awake()
        {
            _staticData = AllServices.Container.Single<IStaticDataService>();
            _chestAnimator.StateEntered += OnStateEntered;
        }

        public void ResetHit() =>
            IsHit = false;

        public void SetReward(int id) =>
            _rewardId = id;

        private void OnStateEntered(AnimatorState state)
        {
            if (state == AnimatorState.Open)
            {
                _chestAnimator.StateEntered -= OnStateEntered;   
                foreach (var particle in particleSystems)
                    particle.Play();
                
                SpawnReward(_rewardId);
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            Shovel shovel = other.GetComponent<Shovel>();

            if (shovel)
            {
                shovel.TakeDamage(9999);
                
                if(_isBossChest)
                    _chestAnimator.PlayOpen();
                else
                    _chestAnimator.PlayHit();

                IsHit = true;
            }
        }

        private void SpawnReward(int id)
        {
            var newView = Instantiate(_staticData.ForCellContent(id).CellContentPrefab, _rewardViewPlace);
            newView.GetComponent<Collider>().enabled = false;
            newView.GetComponent<MergebleObject>()?.StopInteractable();
        }
    }
}
