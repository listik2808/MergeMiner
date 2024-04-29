using System;
using System.Collections;
using Source.Scripts.Infrastructure.Factory;
using Source.Scripts.SaveSystem;
using UnityEngine;
//using Agava.YandexGames;
using Source.Scripts.Analytics;
using Source.Scripts.Infrastructure.Services.StaticData;

namespace Source.Scripts.Infrastructure.States
{
    public class ShovelRewardState : IPayloadedState<int>
    {
        private readonly ICoroutineRunner _runner;
        private readonly IGameStateMachine _stateMachine;
        private readonly IStaticDataService _staticData;
        private readonly IGameFactory _factory;
        private readonly IStorage _storage;
        private readonly IAnalyticManager _analytic;

        private int _rewardId;

        public ShovelRewardState(ICoroutineRunner runner, IGameStateMachine stateMachine, IStaticDataService staticData, IGameFactory factory, IStorage storage, IAnalyticManager analytic)
        {
            _runner = runner;
            _stateMachine = stateMachine;
            _staticData = staticData;
            _factory = factory;
            _storage = storage;
            _analytic = analytic;
        }

        public void Enter(int id)
        {
            _runner.StartCoroutine(Delay(0.6f, () =>
            {
                _rewardId = id;
                _factory.UI.RewardMenu.Header.SetLevelNumberText(_storage.GetInt(DataNames.MaxMergedShovelLevel));
                _factory.UI.RewardMenu.Show(_rewardId);
                _factory.UI.RewardMenu.BackToPlayButton.onClick.AddListener(OnBackToPlayButtonClicked);
                _factory.UI.RewardMenu.RewardButton.onClick.AddListener(OnRewardButtonClicked);
                _factory.Grid.DisableGridContent();
                _analytic.SendEvent(AnalyticNames.ShovelMergeRewardAd + AnalyticNames.Offer);
            }));
        }

        public void Exit()
        {
            _factory.UI.RewardMenu.BackToPlayButton.onClick.RemoveListener(OnBackToPlayButtonClicked);
            _factory.UI.RewardMenu.RewardButton.onClick.RemoveListener(OnRewardButtonClicked);
            _factory.UI.RewardMenu.Hide();
        } 

        private void OnRewardButtonClicked()
        {
            _analytic.SendEvent(AnalyticNames.ShovelMergeRewardAd + AnalyticNames.Click);
#if UNITY_EDITOR
            GetReward();
            OnCloseCallback();
#endif
#if YANDEX_GAMES && !UNITY_EDITOR
            VideoAd.Show(SetPause,GetReward, OnCloseCallback, OnErrorCallback);
#endif
        }

        private void GetReward()
        {
            _storage.SetInt(DataNames.VideoAdShownCount, _storage.GetInt(DataNames.VideoAdShownCount) + 1);
            
            int[] ids = _staticData.ForShovelReward(_rewardId).СellContentIds;

            foreach (var id in ids)
                _factory.Grid.AddContent(id);
            
            _factory.Grid.UpdateProgress();
        }

        private void OnCloseCallback()
        {
            UnPause();
            ReturnToState();
        }

        private void OnErrorCallback(string message)
        {
            UnPause();
            ReturnToState();
        }

        private void OnBackToPlayButtonClicked() =>
            ReturnToState();

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

        private void ReturnToState()
        {
            if(_storage.GetInt(DataNames.IsTutorialComplete).ToBool())
                _stateMachine.Enter<MergeState>();
            else
                _stateMachine.Enter<TutorialPhase3State>();
        }
        
        private IEnumerator Delay(float time, Action callback)
        {
            yield return new WaitForSeconds(time);
            callback?.Invoke();
        }
    }
}
