using GameAnalyticsSDK;
using Source.Scripts.Infrastructure.States;
using UnityEngine;
#if YANDEX_GAMES && !UNITY_EDITOR
using System.Collections;
using Agava.YandexGames;
#endif

namespace Source.Scripts.Infrastructure
{
    public class GameBootstrapper : MonoBehaviour , ICoroutineRunner
    {
        private Game _game;

#if YANDEX_GAMES && !UNITY_EDITOR
        private IEnumerator Start()
        {
            yield return YandexGamesSdk.Initialize();
            GameAnalytics.Initialize();
            _game = new Game(this);
            _game.StateMachine.Enter<BootstrapState>();
            
            DontDestroyOnLoad(this);
        }
#else
        private void Start()
        {
            GameAnalytics.Initialize();
            _game = new Game(this);
            _game.StateMachine.Enter<BootstrapState>();
            DontDestroyOnLoad(this);
        }
#endif
    }
}
