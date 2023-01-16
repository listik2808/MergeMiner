using System;
using Source.Scripts.Infrastructure.Services;
using Source.Scripts.Infrastructure.States;
using Source.Scripts.SaveSystem;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Source.Scripts.UI.OfflineReward
{
    public class OfflineRewardView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _text;
        [SerializeField] private Button _button;
        [SerializeField] private TextMeshProUGUI _multiplierText;
        
        private IStorage _storage;
        private IGameStateMachine _stateMachine;

        private DateTime _testTime;

        public Transform PigPoint => _button.transform;

        private void Awake()
        {
            _storage = AllServices.Container.Single<IStorage>();
            _stateMachine = AllServices.Container.Single<IGameStateMachine>();
            
            if(_storage.GetInt(DataNames.IsTutorialComplete).ToBool() == false)
                gameObject.SetActive(false);
        }
        
        private void OnEnable() => 
            _button.onClick.AddListener(OnClick);

        private void OnDisable() => 
            _button.onClick.RemoveListener(OnClick);

        private void Update()
        {
            if(_storage.GetInt(DataNames.IsAdShowing).ToBool() == false)
                ShowTimerText();
        }

        private void OnClick() => 
            _stateMachine.Enter<OfflineRewardState, bool>(true);

        private void ShowTimerText()
        {
            TimeSpan timeSinceLastRewardTaken = DateTime.Now - _storage.GetLastRewardTakingTime();
            
            TimeSpan timeToReward = new TimeSpan(0,0,0);
            _multiplierText.text = "X4";
            if (timeSinceLastRewardTaken < new TimeSpan(3, 0, 0))
            {
                timeToReward = new TimeSpan(3,0,0) - timeSinceLastRewardTaken;
                _button.interactable = false;
                _multiplierText.text = "X1";
            }
            else if(timeSinceLastRewardTaken < new TimeSpan(6, 0, 0))
            {
                timeToReward = new TimeSpan(6,0,0) - timeSinceLastRewardTaken;
                _button.interactable = true;
                _multiplierText.text = "X2";
            }
            else if (timeSinceLastRewardTaken < new TimeSpan(9, 0, 0))
            {
                timeToReward = new TimeSpan(9,0,0) - timeSinceLastRewardTaken;
                _button.interactable = true;
                _multiplierText.text = "X3";
            }
            else if(timeSinceLastRewardTaken < new TimeSpan(12, 0, 0))
            {
                timeToReward = new TimeSpan(12,0,0) - timeSinceLastRewardTaken;
                _button.interactable = true;
                _multiplierText.text = "X4";
            }

            _text.text = $"{timeToReward.Hours:00}:{timeToReward.Minutes:00}:{timeToReward.Seconds:00}";
        }
    }
}
