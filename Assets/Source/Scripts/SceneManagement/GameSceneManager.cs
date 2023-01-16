using Source.Scripts.Infrastructure.Services;
using Source.Scripts.Infrastructure.States;
using UnityEngine;

namespace Source.Scripts.SceneManagement
{
    public class GameSceneManager : MonoBehaviour
    {
        private IGameStateMachine _gameStateMachine;
        private IExitableState _beforePauseState;

        private void Start() =>
            _gameStateMachine = AllServices.Container.Single<IGameStateMachine>();

        private void OnApplicationQuit() =>
            _gameStateMachine.Enter<ApplicationQuitState>();
    }
}