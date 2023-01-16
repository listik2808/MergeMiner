using System.Collections;
using Source.Scripts.Infrastructure.Factory;
using Source.Scripts.Logic.GoalChecker;
using Source.Scripts.Logic.MainCamera;
using Source.Scripts.SaveSystem;
using UnityEngine;

namespace Source.Scripts.Infrastructure.States
{
    public class GoalPreviewState : IState
    {
        private readonly ICoroutineRunner _runner;
        private readonly IGameStateMachine _stateMachine;
        private readonly IGameFactory _factory;
        private readonly IStorage _storage;

        public GoalPreviewState(ICoroutineRunner runner, IGameStateMachine stateMachine, IGameFactory factory, IStorage storage)
        {
            _runner = runner;
            _stateMachine = stateMachine;
            _factory = factory;
            _storage = storage;
        }

        public void Enter() =>
            _runner.StartCoroutine(GoalShowing(_factory.GoalChecker.Goal));

        public void Exit()
        {
        }

        private IEnumerator GoalShowing(GoalBase goal)
        {
            CameraTracking camera = Object.FindObjectOfType<CameraTracking>();
            _factory.UI.PlayMenu.Hide();
            _factory.UI.MergeMenu.Hide();
            
            switch (goal)
            {
                case ChestGoal:
                    yield return new WaitForSeconds(0.3f);
                    _factory.UI.GoalPreviewMenu.ShowFor(goal, _storage.GetDisplayedLevelNumber());
                    yield return new WaitForSeconds(1.2f);
                    break;
                case BossGoal:
                    yield return new WaitForEndOfFrame();
                    camera.MoveToFinishPosition();
                    yield return new WaitForSeconds(1.2f);
                    _factory.UI.GoalPreviewMenu.ShowFor(goal, _storage.GetDisplayedLevelNumber());
                    yield return new WaitForSeconds(1.8f);
                    camera.MoveToDefaultPosition();
                    break;
            }
            
            _factory.UI.GoalPreviewMenu.Hide();
            _factory.UI.PlayMenu.Show();
            yield return new WaitForSeconds(0.5f);
            
            if(_storage.GetInt(DataNames.IsTutorialComplete).ToBool())
                _stateMachine.Enter<MergeState>();
            else
                _stateMachine.Enter<TutorialPhase1State>();
        }
    }
}