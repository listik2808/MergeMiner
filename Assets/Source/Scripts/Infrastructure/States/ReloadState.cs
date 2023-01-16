using Source.Scripts.Analytics;
using Source.Scripts.Infrastructure.Factory;
using Source.Scripts.Infrastructure.Services.StaticData;
using Source.Scripts.SaveSystem;

namespace Source.Scripts.Infrastructure.States
{
    public class ReloadState : GameWorldInitializer , IState
    {
        private readonly GameStateMachine _stateMachine;
        private readonly SceneLoader _sceneLoader;
        private readonly IStorage _storage;
        private readonly IAnalyticManager _analytic;
        private readonly IStaticDataService _staticData;

        public ReloadState(GameStateMachine stateMachine, SceneLoader sceneLoader, IStorage storage, IAnalyticManager analytic, IGameFactory gameFactory, IStaticDataService staticData) : base(gameFactory, storage, staticData)
        {
            _stateMachine = stateMachine;
            _sceneLoader = sceneLoader;
            _storage = storage;
            _analytic = analytic;
            _staticData = staticData;
        }

        public void Enter()
        {
            //_gameFactory.Cleanup();
            _sceneLoader.Load(_staticData.ForLevel(_storage.GetLevel()).SceneName, OnLoaded);
        }

        public void Exit()
        {
        }

        private void OnLoaded()
        {
            SaveAndSendAnalytic();
            
            InitGameWorld();
            
            _stateMachine.Enter<GoalPreviewState>();
        }

        private void SaveAndSendAnalytic()
        {
#if UNITY_WEBGL
            _storage.SaveRemote();
#else
            _storage.Save();
#endif
            _analytic.SendEventOnLevelRestart(_storage.GetDisplayedLevelNumber());
        }
    }
}