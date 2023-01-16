using Source.Scripts.Infrastructure.Factory;
using Source.Scripts.SaveSystem;

namespace Source.Scripts.Infrastructure.States
{
    public class TutorialPhase3State : IState
    {
        private readonly IGameFactory _factory;
        private readonly GameStateMachine _stateMachine;
        private readonly IStorage _storage;

        public TutorialPhase3State(GameStateMachine stateMachine, IGameFactory factory, IStorage storage)
        {
            _stateMachine = stateMachine;
            _factory = factory;
            _storage = storage;
        }

        public void Enter()
        {
            _factory.Grid.ShovelSpawned += OnShovelSpawned;
            _factory.UI.MergeMenu.Show();
            _factory.UI.MergeMenu.DropShovelButton.Button.interactable = true;
            _factory.UI.TutorialCanvas.HandPointer.Show();
            ShowTutorial();
            _factory.Grid.EnableGridContent();
            _factory.UI.MergeMenu.DropShovelButton.Button.onClick.AddListener(OnDropButtonClick);
        }

        public void Exit()
        {
            _factory.UI.MergeMenu.DropShovelButton.Button.onClick.RemoveListener(OnDropButtonClick);
            _factory.Grid.DisableGridContent();
            _factory.UI.MergeMenu.Hide();
            _factory.UI.TutorialCanvas.HandPointer.Hide();
            _factory.Grid.ShovelSpawned -= OnShovelSpawned;
        }

        private void OnDropButtonClick()
        {
            _factory.Grid.DropShovels();
            _stateMachine.Enter<DropState>();
            _storage.SetInt(DataNames.IsTutorialComplete, true.ToInt());
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

        private void ShowTutorial() =>
            _factory.UI.TutorialCanvas.HandPointer.ShowClickAnimationIn(_factory.UI.MergeMenu.DropShovelButton.HandPointerPosition);
    }
}