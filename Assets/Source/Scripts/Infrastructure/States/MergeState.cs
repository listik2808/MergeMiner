using Source.Scripts.Infrastructure.Factory;
using Source.Scripts.SaveSystem;
using UnityEngine.Events;

namespace Source.Scripts.Infrastructure.States
{
    public class MergeState : IState
    {
        private readonly IGameFactory _factory;

        private readonly GameStateMachine _stateMachine;
        private readonly IStorage _storage;

        public MergeState(GameStateMachine stateMachine, IGameFactory factory, IStorage storage)
        {
            _stateMachine = stateMachine;
            _factory = factory;
            _storage = storage;
        }

        public void Enter()
        {
            _factory.Grid.ShovelSpawned += OnShovelSpawned;
            _factory.UI.PlayMenu.Show();
            _factory.UI.MergeMenu.Show();
            _factory.UI.MergeMenu.DropShovelButton.Button.onClick.AddListener(OnDropButtonClick);
            _factory.UI.MergeMenu.BuyShovelButton.Button.onClick.AddListener(OnBuyShovelButtonClick);
            _factory.Grid.EnableGridContent();
        }

        public void Exit()
        {
            _factory.Grid.DisableGridContent();
            _factory.UI.MergeMenu.DropShovelButton.Button.onClick.RemoveListener(OnDropButtonClick);
            _factory.UI.MergeMenu.BuyShovelButton.Button.onClick.RemoveListener(OnBuyShovelButtonClick);
            _factory.UI.MergeMenu.Hide();
            _factory.Grid.ShovelSpawned -= OnShovelSpawned;
        }

        private void OnShovelSpawned(int id)
        {
            var maxShovelId = _storage.GetInt(DataNames.MaxMergedShovelLevel);

            if (id > maxShovelId)
            {
                _storage.SetInt(DataNames.MaxMergedShovelLevel, id);
                _storage.Save();

                _stateMachine.Enter<ShovelRewardState, int>(id);
            }
        }

        private void OnDropButtonClick()
        {
            _factory.Grid.DropShovels();
            _stateMachine.Enter<DropState>();
        }

        private void OnBuyShovelButtonClick() =>
            _factory.Grid.TryBuyShovel();
    }
}