using System;
using UnityEngine;

public class TimerPartikleUp : MonoBehaviour
{
    [SerializeField] private float _timeUpIncam;

    private float _currentTime;

    private void Awake() => 
        StopTimer();

    private void Update() => 
        Timer();

    public event Action IsPlay;

    public void StopTimer() => 
        enabled = false;

    public void StartTimer() => 
        enabled = true;

    private void Timer()
    {
        _currentTime += Time.deltaTime;

        if (_currentTime >= _timeUpIncam)
        {
            IsPlay.Invoke();
            _currentTime = 0;
        }
    }
}