using System;
using System.Collections;
//using Agava.YandexGames;
using Source.Scripts.Analytics;
using Source.Scripts.Infrastructure.Factory;
using Source.Scripts.SaveSystem;
using UnityEngine;

namespace Source.Scripts.Infrastructure.States
{
    public class FailState : IState
    {
        private readonly ICoroutineRunner _runner;
        private readonly IGameStateMachine _stateMachine;
        private readonly IGameFactory _factory;
        private readonly IStorage _storage;
        private readonly IAnalyticManager _analytic;

        public FailState(ICoroutineRunner runner, IGameStateMachine stateMachine, IGameFactory factory, IStorage storage, IAnalyticManager analytic)
        {
            _runner = runner;
            _stateMachine = stateMachine;
            _factory = factory;
            _storage = storage;
            _analytic = analytic;
        }
        
        public void Enter()
        {
            _runner.StartCoroutine(Delay(1f, () =>
            {
                _storage.SetInt(DataNames.BadAttemptsNumber, _storage.GetInt(DataNames.BadAttemptsNumber) + 1);
                ReInitGameWorld();
#if YANDEX_GAMES && !UNITY_EDITOR
            InterstitialAd.Show(
                SetPause,
                (a) => UnPause(),
                (a) => UnPause());
#endif
                SaveAndSendAnalytic();
                _stateMachine.Enter<MergeState>();
            }));
        }

        public void Exit()
        {
        }

        private void SaveAndSendAnalytic()
        {
#if UNITY_WEBGL
            _storage.SaveRemote();
#else
            //Storage.Save();
#endif
            _analytic.SendEventOnFail(_storage.GetDisplayedLevelNumber());
        }

        private void ReInitGameWorld()
        {
            _factory.CameraTracking.MoveToDefaultPosition();
            _factory.Grid.RespawnGridContent();
            _factory.LevelElements.Reactivate();
            _factory.GoalChecker.Goal.ResetGoal();
        }

        private IEnumerator Delay(float time, Action callback)
        {
            yield return new WaitForSeconds(time);
            callback?.Invoke();
        }
        
        private void SetPause()
        {
            PlayerPrefs.SetInt(DataNames.IsAdShowing, true.ToInt());
            _analytic.SendEvent(AnalyticNames.InterstitialAd);
            Time.timeScale = 0;
            AudioListener.pause = true;
            AudioListener.volume = 0f;
        }
        
        private void UnPause()
        {
            PlayerPrefs.SetInt(DataNames.IsAdShowing, false.ToInt());
            Time.timeScale = 1;
            AudioListener.pause = false;
            AudioListener.volume = 1f;
        }
    }
}