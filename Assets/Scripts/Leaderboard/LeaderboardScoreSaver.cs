using System;
using Agava.YandexGames;
using Source.Scripts.Infrastructure.Services;
using Source.Scripts.SaveSystem;
using UnityEngine;

public class LeaderboardScoreSaver : MonoBehaviour
{
    [SerializeField] private float _timeBetweenSaves;

    private int _totalScore;
    private int _lastSavedScore;
    private IStorage _storage;

    private void Awake()
    {
        _storage = AllServices.Container.Single<IStorage>();
    }

    public void Load()
    {
        //_totalScore = PlayerPrefs.GetInt(nameof(_totalScore), 0);
        _totalScore = _storage.GetDisplayedLevelNumber();
        
        _lastSavedScore = _totalScore;
    }

    public void StashScore(int score)
    {
        //_totalScore += score;

        _totalScore = _storage.GetDisplayedLevelNumber();
        Save();
    }

    private void OnCompleted()
    {
        Save();
    }

    private void Save()
    {
        //PlayerPrefs.SetInt(nameof(_totalScore), _totalScore);

#if !UNITY_EDITOR && UNITY_WEBGL
        if (PlayerAccount.IsAuthorized && _lastSavedScore != _totalScore)
        {
            Leaderboard.SetScore(LeaderboardName.Name, _totalScore);
            _lastSavedScore = _totalScore;
        }
#endif
    }
}
