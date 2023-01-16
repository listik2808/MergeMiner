using Source.Scripts.Infrastructure.Factory;
using Source.Scripts.SaveSystem;

namespace Source.Scripts.Infrastructure.States
{
    public class TutorialPhase1State : IState
    {
        private readonly IGameFactory _factory;
        private readonly GameStateMachine _stateMachine;
        private readonly IStorage _storage;

        public TutorialPhase1State(GameStateMachine stateMachine, IGameFactory factory, IStorage storage)
        {
            _stateMachine = stateMachine;
            _factory = factory;
            _storage = storage;
        }

        public void Enter()
        {
            _factory.Grid.ShovelSpawned += OnShovelSpawned;
            _factory.UI.MergeMenu.Show();
            _factory.UI.MergeMenu.DropShovelButton.Button.interactable = false;
            _factory.UI.MergeMenu.BuyShovelButton.Button.onClick.AddListener(OnBuyShovelButtonClick);
            _factory.UI.TutorialCanvas.HandPointer.Show();
            _storage.SetSoft(15);
            ShowTutorial();
            _factory.Grid.EnableGridContent();
        }

        public void Exit()
        {
            _factory.UI.MergeMenu.BuyShovelButton.Button.onClick.RemoveListener(OnBuyShovelButtonClick);
            _factory.UI.TutorialCanvas.HandPointer.Hide();
            _factory.Grid.ShovelSpawned -= OnShovelSpawned;
        }

        private void OnBuyShovelButtonClick() =>
            _factory.Grid.TryBuyShovel();

        private void OnShovelSpawned(int id) =>
            _stateMachine.Enter<TutorialPhase2State>();

        private void ShowTutorial() =>
            _factory.UI.TutorialCanvas.HandPointer.ShowClickAnimationIn(_factory.UI.MergeMenu.BuyShovelButton.HandPointerPosition);
    }
}