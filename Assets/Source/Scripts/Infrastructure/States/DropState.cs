using Source.Scripts.Infrastructure.Factory;
using Source.Scripts.Logic.MainCamera;
using UnityEngine;

namespace Source.Scripts.Infrastructure.States
{
    public class DropState : IState
    {
        private readonly IGameFactory _gameFactory;

        public DropState(IGameFactory gameFactory)
        {
            _gameFactory = gameFactory;
        }
        
        public void Enter()
        {
            Object.FindObjectOfType<CameraTracking>().StartTrack(_gameFactory.Grid.Shovels);
            
            _gameFactory.LevelElements.EnableCubesColliders();
            _gameFactory.GoalChecker.Enable();
        }

        public void Exit()
        {
            _gameFactory.LevelElements.DisableCubesColliders();
            _gameFactory.Grid.UpdateProgress();
            _gameFactory.GoalChecker.Disable();
        }
    }
}