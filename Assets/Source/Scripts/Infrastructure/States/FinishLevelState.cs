using System;
using System.Collections;
using System.Collections.Generic;
using Agava.YandexGames;
using Source.Scripts.Analytics;
using Source.Scripts.Infrastructure.Factory;
using Source.Scripts.Infrastructure.Services.StaticData;
using Source.Scripts.Logic.MainCamera;
using Source.Scripts.SaveSystem;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Source.Scripts.Infrastructure.States
{
    public class FinishLevelState : IPayloadedState<IReadOnlyList<int>>
    {
        private readonly IGameFactory _factory;
        private readonly IStorage _storage;
        private readonly IStaticDataService _staticData;
        private readonly IAnalyticManager _analytic;
        private readonly ICoroutineRunner _runner;
        private readonly IGameStateMachine _gameStateMachine;

        public FinishLevelState(ICoroutineRunner runner, IGameStateMachine gameStateMachine, IGameFactory factory, IStorage storage, IStaticDataService staticData, IAnalyticManager analytic)
        {
            _runner = runner;
            _gameStateMachine = gameStateMachine;
            _factory = factory;
            _storage = storage;
            _staticData = staticData;
            _analytic = analytic;
        }

        public void Enter(IReadOnlyList<int> rewardIds)
        {
            Object.FindObjectOfType<CameraTracking>().MoveToFinishPosition();

            _runner.StartCoroutine(Delay(1.5f, () =>
            {
                _storage.SetInt(DataNames.BadAttemptsNumber, 0);
                AddChestRewardsToGrid(rewardIds);
                _factory.Grid.UpdateProgress();
                _factory.UI.LevelCompleteMenu.Header.SetLevelNumberText(_storage.GetDisplayedLevelNumber());
                _factory.UI.LevelCompleteMenu.Show(rewardIds);
                _factory.UI.LevelCompleteMenu.NextButton.onClick.AddListener(OnNextButtonClicked);
                _factory.UI.LevelCompleteMenu.RewardButton.Closed += OnClose;
                _analytic.SendEvent(AnalyticNames.LevelCompleteRewardAd + AnalyticNames.Offer);
            }));
        }

        public void Exit()
        {
            SetLeaderboardScore();
            _storage.Save();
            _factory.UI.LevelCompleteMenu.NextButton.onClick.RemoveListener(OnNextButtonClicked);
            _factory.UI.LevelCompleteMenu.RewardButton.Closed -= OnClose;
        }

        private void OnNextButtonClicked()
        {
            _storage.SetSoft(_storage.GetSoft() + _staticData.ForLevel(_storage.GetLevel()).LevelReward);
            StartNextScene();
        }

        private void OnClose()
        {
            PlayerPrefs.SetInt(DataNames.IsVideoAdShown, true.ToInt());
            _analytic.SendEvent(AnalyticNames.LevelCompleteRewardAd + AnalyticNames.Click);
            StartNextScene();
        }

        private void StartNextScene()
        {
            _analytic.SendEventOnLevelComplete(_storage.GetDisplayedLevelNumber());
            
            _gameStateMachine.Enter<LoadNextLevelState>();
        }

        private void AddChestRewardsToGrid(IReadOnlyList<int> rewardIds)
        {
            foreach (var id in rewardIds)
                _factory.Grid.AddContent(id);
        }

        private void SetLeaderboardScore()
        {
#if YANDEX_GAMES && !UNITY_EDITOR
            Leaderboard.SetScore(LeaderboardName.Name, _storage.GetDisplayedLevelNumber());
#endif
        }
        
        private IEnumerator Delay(float time, Action callback)
        {
            yield return new WaitForSeconds(time);
            callback?.Invoke();
        }
    }
}