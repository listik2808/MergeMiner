using Source.Scripts.Infrastructure.Factory;
using Source.Scripts.SaveSystem;

namespace Source.Scripts.Infrastructure.States
{
    public class TutorialPhase2State : IState
    {
        private readonly IGameFactory _factory;
        private readonly GameStateMachine _stateMachine;
        private readonly IStorage _storage;

        public TutorialPhase2State(GameStateMachine stateMachine, IGameFactory factory, IStorage storage)
        {
            _stateMachine = stateMachine;
            _factory = factory;
            _storage = storage;
        }

        public void Enter()
        {
            _factory.Grid.ShovelSpawned += OnShovelSpawned;
            _factory.UI.MergeMenu.DropShovelButton.Button.interactable = false;
            _factory.UI.TutorialCanvas.HandPointer.Show();
            ShowTutorial();
        }

        public void Exit()
        {
            _factory.Grid.DisableGridContent();
            _factory.UI.MergeMenu.Hide();
            _factory.UI.TutorialCanvas.HandPointer.Hide();
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

        private void ShowTutorial()
        {
            _factory.UI.TutorialCanvas.HandPointer.ShowDragAnimation(_factory.Grid.Shovels[1].transform, _factory.Grid.Shovels[0].transform);
        }
    }
}