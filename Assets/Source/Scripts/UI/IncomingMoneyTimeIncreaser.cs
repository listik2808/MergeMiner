using System;
//using Agava.YandexGames;
using Source.Scripts.MergedObjects;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Grid = Source.Scripts.Logic.GridBehaviour.Grid;

namespace Source.Scripts.UI
{
    public class IncomingMoneyTimeIncreaser : MonoBehaviour
    {
        [SerializeField] private Button _timeIncreaseRewardButton;
        [SerializeField] private float _additionalTime;
        [SerializeField] private UI _ui;
        [SerializeField] private TMP_Text _buttonText;
        
        private Grid _grid;
        private Timer _timer;
        private bool _isTimerTicking;
        private string _defaultText;
        
        public event Action TimerUpdated;
        
        private void Awake()
        {
            _timer = new Timer();
            _defaultText = _buttonText.text;
        }

        private void OnEnable()
        {
            _timeIncreaseRewardButton.onClick.AddListener(OnClick);
            _timer.Completed += OnCompleted;
            _timer.Updated += OnTimeUpdate;
        }

        private void OnDisable()
        {
            _grid.ShovelSpawned -= OnShovelSpawned;
            _timeIncreaseRewardButton.onClick.RemoveListener(OnClick);
            _timer.Completed -= OnCompleted;
            _timer.Updated -= OnTimeUpdate;

        }

        private void OnTimeUpdate() => 
            Display();

        private void OnCompleted()
        {
            _timer.Stop();
            _isTimerTicking = false;
            
            //foreach (MergebleObject mergeableObject in _grid.Shovels) 
            //    mergeableObject.Shovel.Timer.ResetTime();
        }

        private void Start()
        {
            _grid = _ui.Grid;
            _grid.ShovelSpawned += OnShovelSpawned;
        }

        private void Update()
        {
            _timer.Tick(Time.deltaTime);
        }

        private void OnShovelSpawned(int id)
        {
            //if (_isTimerTicking)
            //    foreach (MergebleObject mergeableObject in _grid.Shovels)
            //        mergeableObject.Shovel.Timer.ReduceTime();
        }

        private void OnClick()
        {
#if !UNITY_EDITOR && UNITY_WEBGL
            VideoAd.Show(OnOpenCallback, OnRewardedCallback, OnCloseCallback, OnErrorCallback);
#endif
        }

        private void Display()
        {
            float minutes = Mathf.FloorToInt(_timer.TimeLeft / 60);
            float seconds = Mathf.FloorToInt(_timer.TimeLeft % 60);

            _buttonText.text = string.Format("{0:00}:{1:00}", minutes, seconds);

            if(_timer.TimeLeft <= 0) 
                _buttonText.text = _defaultText;
        }

        private void OnOpenCallback() => 
            SetPause();

        private void OnRewardedCallback()
        {
            UpdateTimer();

            //foreach (MergebleObject mergeableObject in _grid.Shovels) 
            //    mergeableObject.Shovel.Timer.ReduceTime();
        }

        private void OnCloseCallback() => 
            UnPause();

        private void OnErrorCallback(string message) => 
            UnPause();

        private void SetPause() => 
            Time.timeScale = 0;

        private void UnPause() => 
            Time.timeScale = 1;


        private void UpdateTimer()
        {
            if (_timer.IsTimerStart)
                _timer.AddTime(_additionalTime);
            else
                _timer.Start(_additionalTime);

            _isTimerTicking = true;
            
            TimerUpdated?.Invoke();
        }
    }
}