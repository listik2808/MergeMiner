using System;
//using Agava.YandexGames;
using Source.Scripts.Analytics;
using Source.Scripts.Infrastructure.Factory;
using Source.Scripts.Infrastructure.Services.StaticData;
using Source.Scripts.SaveSystem;
using UnityEngine;

namespace Source.Scripts.Infrastructure.States
{
    public class LoadLevelState : GameWorldInitializer , IState
    {
        private readonly GameStateMachine _stateMachine;
        private readonly SceneLoader _sceneLoader;
        private readonly IStaticDataService _staticData;
        private readonly IStorage _storage;
        private readonly IAnalyticManager _analytic;
        private readonly IGameFactory _factory;


        public LoadLevelState(GameStateMachine stateMachine, SceneLoader sceneLoader, IStorage storage, IAnalyticManager analytic, IGameFactory factory, IStaticDataService staticData) : base(factory, storage, staticData)
        {
            _stateMachine = stateMachine;
            _sceneLoader = sceneLoader;
            _staticData = staticData;
            _storage = storage;
            _analytic = analytic;
            _factory = factory;
        }

        public void Enter() => 
            _sceneLoader.Load(_staticData.ForLevel(_storage.GetLevel()).SceneName, OnLoaded);

        public void Exit()
        {
        }

        private void OnLoaded()
        {
            SaveAndSendAnalytic();
            _storage.SetInt(DataNames.BadAttemptsNumber, 0);
            PlayerPrefs.SetInt(DataNames.IsVideoAdShown, false.ToInt());
            if(_factory.AudioObject == null)
                _factory.CreateAudio();
            InitGameWorld();
// #if YANDEX_GAMES && !UNITY_EDITOR
//             InterstitialAd.Show(
//                 SetPause,
//                 (a) =>
//                 {
//                     UnPause();
//                 },
//                 (a) =>
//                 {
//                     UnPause();
//                 });
// #endif
            if (DateTime.Now - _storage.GetLastRewardTakingTime() > new TimeSpan(3, 0, 3) && _storage.GetInt(DataNames.IsTutorialComplete).ToBool())
                _stateMachine.Enter<OfflineRewardState, bool>(false);
            else
                _stateMachine.Enter<GoalPreviewState>();
        }

        private void SaveAndSendAnalytic()
        {
            _storage.SetTimeSinceLastLogin();
            _storage.AddSession();
            _storage.SetLastLoginDate();
            _storage.Save();
            _analytic.SendEventOnGameInitialize(_storage.GetSessionCount());
            _analytic.SendEventOnLevelStart(_storage.GetDisplayedLevelNumber());
        }
        
        private void SetPause()
        {
            PlayerPrefs.SetInt(DataNames.IsAdShowing, true.ToInt());
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