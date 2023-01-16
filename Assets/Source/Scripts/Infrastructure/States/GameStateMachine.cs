using System;
using System.Collections.Generic;
using Source.Scripts.Analytics;
using Source.Scripts.Infrastructure.Factory;
using Source.Scripts.Infrastructure.Services;
using Source.Scripts.Infrastructure.Services.StaticData;
using Source.Scripts.SaveSystem;

namespace Source.Scripts.Infrastructure.States
{
    public class GameStateMachine : IGameStateMachine
    {
        private readonly Dictionary<Type, IExitableState> _states;
        private IExitableState _activeState;

        public GameStateMachine(SceneLoader sceneLoader, ICoroutineRunner coroutineRunner, AllServices services)
        {
            _states = new Dictionary<Type, IExitableState>
            {
                [typeof(BootstrapState)] = new BootstrapState(this, sceneLoader, services),
                [typeof(LoadProgressState)] = new LoadProgressState(this, services.Single<IStorage>(), coroutineRunner),
                [typeof(LoadLevelState)] = new LoadLevelState(this, sceneLoader, services.Single<IStorage>(), services.Single<IAnalyticManager>(), services.Single<IGameFactory>(), services.Single<IStaticDataService>()),
                [typeof(ReloadState)] = new ReloadState(this, sceneLoader, services.Single<IStorage>(), services.Single<IAnalyticManager>(), services.Single<IGameFactory>(), services.Single<IStaticDataService>()),
                [typeof(LoadNextLevelState)] = new LoadNextLevelState(this, sceneLoader, services.Single<IStorage>(), services.Single<IAnalyticManager>(), services.Single<IGameFactory>(), services.Single<IStaticDataService>()),
                [typeof(MergeState)] = new MergeState(this, services.Single<IGameFactory>(), services.Single<IStorage>()),
                [typeof(DropState)] = new DropState(services.Single<IGameFactory>()),
                [typeof(FinishLevelState)] = new FinishLevelState(coroutineRunner, this ,services.Single<IGameFactory>(), services.Single<IStorage>(), services.Single<IStaticDataService>(), services.Single<IAnalyticManager>()),
                [typeof(ShovelRewardState)] = new ShovelRewardState(coroutineRunner,this, services.Single<IStaticDataService>(), services.Single<IGameFactory>(), services.Single<IStorage>(), services.Single<IAnalyticManager>()),
                [typeof(FailState)] = new FailState( coroutineRunner,this, services.Single<IGameFactory>(), services.Single<IStorage>(), services.Single<IAnalyticManager>()),
                [typeof(ApplicationQuitState)] = new ApplicationQuitState(services.Single<IStorage>(), services.Single<IAnalyticManager>()),
                [typeof(ApplicationPauseState)] = new ApplicationPauseState(services.Single<IStorage>(), services.Single<IAnalyticManager>()),
                [typeof(GoalPreviewState)] = new GoalPreviewState(coroutineRunner, this, services.Single<IGameFactory>(), services.Single<IStorage>()),
                [typeof(TutorialPhase1State)] = new TutorialPhase1State(this, services.Single<IGameFactory>(), services.Single<IStorage>()),
                [typeof(TutorialPhase2State)] = new TutorialPhase2State(this, services.Single<IGameFactory>(), services.Single<IStorage>()),
                [typeof(TutorialPhase3State)] = new TutorialPhase3State(this, services.Single<IGameFactory>(), services.Single<IStorage>()),
                [typeof(OfflineRewardState)] = new OfflineRewardState(this, services.Single<IStorage>(), services.Single<IGameFactory>(), services.Single<IAnalyticManager>())
            };
        }

        public void Enter<TState>() where TState : class, IState
        {
            TState state = ChangeState<TState>();
            state.Enter();
        }

        public void Enter<TState, TPayload>(TPayload payload) where TState : class, IPayloadedState<TPayload>
        {
            TState state = ChangeState<TState>();
            state.Enter(payload);
        }

        private TState ChangeState<TState>() where TState : class, IExitableState
        {
            _activeState?.Exit();
            TState state = GetState<TState>();
            _activeState = state;
            return state;
        }

        private TState GetState<TState>() where TState : class, IExitableState =>
            _states[typeof(TState)] as TState;
    }
}
