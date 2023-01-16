using Source.Scripts.Infrastructure.Services;
using Source.Scripts.Infrastructure.States;

namespace Source.Scripts.Infrastructure
{
    public class Game
    {
        public GameStateMachine StateMachine;

        public Game(ICoroutineRunner coroutineRunner)
        {
            StateMachine = new GameStateMachine(new SceneLoader(coroutineRunner), coroutineRunner, AllServices.Container);
        }
    }
}