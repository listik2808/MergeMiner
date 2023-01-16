using System;
using UnityEngine;

namespace Shovels
{
    public class ShovelMoneyIncomeTimer : MonoBehaviour
    {
        [SerializeField] private float _currentTime;
        [SerializeField] private float _timeBeetweenMoneyIncome = 1f;
        [SerializeField] private float _timeDelta;
        
        private Timer _timer;
        
        public event Action TimerCompleted;
        
        private void Awake()
        {
            _timer = new Timer();
            ResetTime();
        }

        private void OnEnable() => _timer.Completed += OnTimerCompleted;

        private void OnDisable() => _timer.Completed -= OnTimerCompleted;

        private void Update() => _timer.Tick(Time.deltaTime);

        public void StartTimer() => _timer.Start(_currentTime);

        private void OnTimerCompleted()
        {
            TimerCompleted?.Invoke();
            RestartTimer();
        }
        
        public void StopTimer() => _timer.Stop();
        private void RestartTimer() => StartTimer();
        public void ReduceTime() => _currentTime = _timeBeetweenMoneyIncome / _timeDelta;
        public void ResetTime() => _currentTime = _timeBeetweenMoneyIncome;
        
        
    }
}