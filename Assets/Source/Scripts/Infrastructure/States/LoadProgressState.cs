using System;
using Source.Scripts.SaveSystem;

namespace Source.Scripts.Infrastructure.States
{
    public class LoadProgressState : IState
    {
        private readonly GameStateMachine _gameStateMachine;
        private readonly IStorage _storage;
        private readonly ICoroutineRunner _coroutineRunner;

        public LoadProgressState(GameStateMachine gameStateMachine, IStorage storage, ICoroutineRunner coroutineRunner)
        {
            _gameStateMachine = gameStateMachine;
            _storage = storage;
            _coroutineRunner = coroutineRunner;
        }

        public void Enter()
        {
            LoadProgressOrInitNew(() =>
                _gameStateMachine.Enter<LoadLevelState>());
        }

        public void Exit()
        {
        }

        private void LoadProgressOrInitNew(Action onRemoteDataLoaded)
        {
            //_coroutineRunner.StartCoroutine(_storage.SyncRemoteSave(onRemoteDataLoaded));
        }
    }
}