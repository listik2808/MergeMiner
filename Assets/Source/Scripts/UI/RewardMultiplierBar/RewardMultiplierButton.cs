using System;
using Source.Scripts.SaveSystem;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
#if YANDEX_GAMES && !UNITY_EDITOR
using Agava.YandexGames;
#endif

namespace Source.Scripts.UI.RewardMultiplierBar
{
    public class RewardMultiplierButton : MonoBehaviour
    {
        [SerializeField] private TMP_Text _text;
        [SerializeField] private Button _button;
        [SerializeField] private MultiplierCursor _cursor;
        [SerializeField] private UI _uI;
    
        private string _buttonText;
        private int _rewardedMoney;

        public event Action Closed;

        private void OnEnable()
        {
            _button.onClick.AddListener(OnButtonClick);
            _cursor.MultiplierUpdated += OnMultiplierUpdated;
        }

        private void OnDisable()
        {
            _button.onClick.RemoveListener(OnButtonClick);
            _cursor.MultiplierUpdated -= OnMultiplierUpdated;
        }

        private void OnMultiplierUpdated(int value)
        {
            _rewardedMoney = _uI.CoinsReward * value;
            SetText(value.ToString());
        }

        private void SetText(string multiplierValue) => 
            _text.text = multiplierValue;

        private void OnButtonClick()
        {
            _cursor.Stop();
#if UNITY_EDITOR
            OnRewardedCallback();
            OnCloseCallback();
#endif
        
#if YANDEX_GAMES && !UNITY_EDITOR
            VideoAd.Show(OnOpenCallback, OnRewardedCallback, OnCloseCallback);
#endif
        }

        private void OnOpenCallback()
        {
            PlayerPrefs.SetInt(DataNames.IsAdShowing, true.ToInt());
            Time.timeScale = 0;
            AudioListener.pause = true;
            AudioListener.volume = 0f;
        }

        private void OnCloseCallback()
        {
            PlayerPrefs.SetInt(DataNames.IsAdShowing, false.ToInt());
            Time.timeScale = 1;
            AudioListener.pause = false;
            AudioListener.volume = 1f;
            Closed?.Invoke();
        }

        private void OnRewardedCallback()
        {
            int currentSoft = _uI.Storage.GetSoft();
            int moneyToGet = currentSoft + _rewardedMoney;
            _uI.Storage.SetSoft(moneyToGet);
            _uI.Storage.Save();
        }
    }
}
