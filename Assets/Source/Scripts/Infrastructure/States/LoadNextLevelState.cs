//using Agava.YandexGames;
using Source.Scripts.Analytics;
using Source.Scripts.Infrastructure.Factory;
using Source.Scripts.Infrastructure.Services.StaticData;
using Source.Scripts.SaveSystem;
using UnityEngine;

namespace Source.Scripts.Infrastructure.States
{
    public class LoadNextLevelState : GameWorldInitializer , IState
    {
        private readonly GameStateMachine _stateMachine;
        private readonly SceneLoader _sceneLoader;
        private readonly IStorage _storage;
        private readonly IAnalyticManager _analytic;
        private readonly IGameFactory _factory;
        private readonly IStaticDataService _staticData;

        public LoadNextLevelState(GameStateMachine stateMachine, SceneLoader sceneLoader, IStorage storage, IAnalyticManager analytic, IGameFactory factory, IStaticDataService staticData) : base(factory, storage, staticData)
        {
            _stateMachine = stateMachine;
            _sceneLoader = sceneLoader;
            _storage = storage;
            _analytic = analytic;
            _factory = factory;
            _staticData = staticData;
        }
        
        public void Enter()
        {
            string sceneName = GetNextLevelSceneName();
            SaveAndSendAnalytic();
            
            _sceneLoader.Load(sceneName, OnLoaded);
        }

        public void Exit()
        {
        }

        private void OnLoaded()
        {
            InitGameWorld();
#if YANDEX_GAMES && !UNITY_EDITOR
            if(PlayerPrefs.GetInt(DataNames.IsVideoAdShown).ToBool() == false)
                InterstitialAd.Show(
                    SetPause,
                    (a) => UnPause(),
                    (a) => UnPause());
#endif
            PlayerPrefs.SetInt(DataNames.IsVideoAdShown, false.ToInt());
            
            _stateMachine.Enter<GoalPreviewState>();
        }

        private string GetNextLevelSceneName()
        {
            int nextLevelIndex = _storage.GetLevel() + 1;
            if (_staticData.ForLevel(nextLevelIndex) == null)
            {
                _analytic.SendEventContentIsOver(
                    _storage.GetSessionCount(), 
                    _storage.GetNumberDaysAfterRegistration());

                int repeatGameFromLevelIndex = _staticData.ForRepeatGameFromLevelNumber();
                _storage.SetLevel(repeatGameFromLevelIndex);
                return _staticData.ForLevel(repeatGameFromLevelIndex).SceneName;
            }

            _storage.SetLevel(nextLevelIndex);
            return _staticData.ForLevel(nextLevelIndex).SceneName;
        }

        private void SaveAndSendAnalytic()
        {
            _storage.AddDisplayedLevelNumber();
#if UNITY_WEBGL
            _storage.SaveRemote();
#else
           // Storage.Save();
#endif
            _analytic.SendEventOnLevelStart(_storage.GetDisplayedLevelNumber());
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