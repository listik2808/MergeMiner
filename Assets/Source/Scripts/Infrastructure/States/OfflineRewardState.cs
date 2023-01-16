using System;
using System.Collections.Generic;
using Agava.YandexGames;
using Source.Scripts.Analytics;
using Source.Scripts.Infrastructure.Factory;
using Source.Scripts.SaveSystem;
using UnityEngine;

namespace Source.Scripts.Infrastructure.States
{
    public class OfflineRewardState : IPayloadedState<bool>
    {
        private readonly IGameStateMachine _stateMachine;
        private readonly IStorage _storage;
        private readonly IGameFactory _factory;
        private readonly IAnalyticManager _analytic;

        private bool _isNextStateMergeState;
        private List<int> _rewardIds = new List<int>();


        public OfflineRewardState(IGameStateMachine stateMachine, IStorage storage, IGameFactory factory, IAnalyticManager analytic)
        {
            _stateMachine = stateMachine;
            _storage = storage;
            _factory = factory;
            _analytic = analytic;
        }
        
        public void Enter(bool isNextStateMergeState)
        {
            _isNextStateMergeState = isNextStateMergeState;
            CreateRewardsList();
            _factory.UI.PlayMenu.Hide();
            _factory.UI.MergeMenu.Hide();
            _factory.UI.OfflineRewardMenu.Show();
            CreateRewardViews();
            _analytic.SendEvent(AnalyticNames.OfflineRewardAd + AnalyticNames.Offer);
            _factory.UI.OfflineRewardMenu.NextButton.onClick.AddListener(OnReturnToGame);
            _factory.UI.OfflineRewardMenu.RewardButton.onClick.AddListener(OnRewardButtonClick);
        }

        public void Exit()
        {
            _factory.UI.OfflineRewardMenu.NextButton.onClick.RemoveListener(OnReturnToGame);
            _factory.UI.OfflineRewardMenu.RewardButton.onClick.RemoveListener(OnRewardButtonClick);
            _factory.UI.OfflineRewardMenu.ClearRewardView();
            _rewardIds.Clear();
            _factory.UI.OfflineRewardMenu.Hide();
        }

        private void OnRewardButtonClick()
        {
            _analytic.SendEvent(AnalyticNames.OfflineRewardAd + AnalyticNames.Click);
#if UNITY_EDITOR
            GetExtraReward();
            OnReturnToGame();
#endif
#if YANDEX_GAMES && !UNITY_EDITOR
            VideoAd.Show(SetPause, GetExtraReward, OnReturnToGame, (a)=>OnReturnToGame());
#endif
        }

        private void OnReturnToGame()
        {
            UnPause();
            GetCommonReward();
            
            if(_isNextStateMergeState)
                _stateMachine.Enter<MergeState>();
            else
                _stateMachine.Enter<GoalPreviewState>();
        }

        private void GetExtraReward()
        {
            _rewardIds.Clear();
            AddRewardWith(2);
            AddRewardWith(2);
            AddRewardWith(1);
            AddRewardWith(0);
            
            _storage.SetInt(DataNames.VideoAdShownCount, _storage.GetInt(DataNames.VideoAdShownCount) + 1);
        }

        private void GetCommonReward()
        {
            foreach (var id in _rewardIds)
                _factory.Grid.AddContent(id);
            
            _storage.SetLastRewardTakingTime();
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

        private void CreateRewardViews()
        {
            foreach (var id in _rewardIds)
                _factory.UI.OfflineRewardMenu.CreateRewardView(id);
        }
        
        private void CreateRewardsList()
        {
            TimeSpan timeSinceLastRewardTaken = DateTime.Now - _storage.GetLastRewardTakingTime();
            
            if (timeSinceLastRewardTaken > new TimeSpan(3, 0, 0))
                AddRewardWith(2);
            
            if (timeSinceLastRewardTaken > new TimeSpan(6, 0, 0))
                AddRewardWith(2);
            
            if (timeSinceLastRewardTaken > new TimeSpan(9, 0, 0))
                AddRewardWith(1);
            
            if (timeSinceLastRewardTaken > new TimeSpan(12, 0, 0))
                AddRewardWith(0);
        }

        private void AddRewardWith(int decrement)
        {
            int id = _storage.GetInt(DataNames.MaxMergedShovelLevel) - decrement;
            id = Mathf.Clamp(id, 1, _storage.GetInt(DataNames.MaxMergedShovelLevel));
            _rewardIds.Add(id);
        }
    }
}
