using System.Collections.Generic;
using Source.Scripts.Analytics;
using Source.Scripts.Infrastructure.AssetManagement;
using Source.Scripts.Infrastructure.Factory;
using Source.Scripts.Infrastructure.Services;
using Source.Scripts.Infrastructure.Services.Input;
using Source.Scripts.Infrastructure.Services.StaticData;
using Source.Scripts.SaveSystem;
using UnityEngine;
using Application = UnityEngine.Device.Application;

namespace Source.Scripts.Infrastructure.States
{
    public class BootstrapState : IState
    {
        private const string InitialSceneName = "000_LoadingScene";
        private readonly GameStateMachine _stateMachine;
        private readonly AllServices _services;
        private readonly SceneLoader _sceneLoader;

        public BootstrapState(GameStateMachine stateMachine, SceneLoader sceneLoader, AllServices services)
        {
            _stateMachine = stateMachine;
            _sceneLoader = sceneLoader;
            _services = services;

            PlayerPrefs.SetInt(DataNames.IsAdShowing, false.ToInt());
            
            RegisterServices();
        }

        public void Enter() =>
            //_sceneLoader.Load(InitialSceneName, onLoaded: EnterLoadProgressState);
            EnterLoadProgressState();

        private void EnterLoadProgressState() =>
            _stateMachine.Enter<LoadProgressState>();

        public void Exit()
        {
        }

        private void RegisterServices()
        {
            RegisterStaticDataService();
            _services.RegisterSingle<IGameStateMachine>(_stateMachine);
            _services.RegisterSingle<IStorage>(new Storage(DataNames.GameName));
            _services.RegisterSingle<IAnalyticManager>(CreateAnalyticManager());
            _services.RegisterSingle<IAssetProvider>(new AssetProvider());
            _services.RegisterSingle<IGameFactory>(new GameFactory(
                AllServices.Container.Single<IAssetProvider>(), 
                AllServices.Container.Single<IStorage>(),
                AllServices.Container.Single<IStaticDataService>(),
                AllServices.Container.Single<IGameStateMachine>()));
        }

        private void RegisterStaticDataService()
        {
            IStaticDataService staticData = new StaticDataService();
            staticData.LoadGameConfig();
            staticData.LoadCellContentInfo();
            staticData.LoadRewardForShovelConfigs();
            _services.RegisterSingle(staticData);
        }

        private IAnalyticManager CreateAnalyticManager()
        {
            IAnalyticManager analytic = new AnalyticManager(new List<IAnalytic>
            {
#if GAME_ANALYTICS
                new GameAnalyticsAnalytic()
#endif
            });
#if YANDEX_METRICA && !UNITY_EDITOR
            analytic.AddAnalytic(new YandexMetricaAnalytic());
#endif
            return analytic;
        }
    }
}