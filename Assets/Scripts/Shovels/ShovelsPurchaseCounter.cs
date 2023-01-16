using Source.Scripts.Infrastructure.Services;
using Source.Scripts.SaveSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShovelsPurchaseCounter : MonoBehaviour
{
    [SerializeField] private Grid _grid;
    [SerializeField] private int _maxAmount;

    private int _currentAmount;
    private IStorage _storage;
    private int _currentShovelLevel = 0;

    public event Action ShowedScore;

    public int CurrentShovelLevel => _currentShovelLevel;
    public int CurrentAmount => _currentAmount;

    private void Awake()
    {
        _storage = AllServices.Container.Single<IStorage>();
    }

    public void IncreaseNumberPurchasedShovels()
    {
        _currentAmount++;

        if (_currentAmount >= _maxAmount)
        {
            _currentShovelLevel++;
            AddMaxCount();
        }

        ShowedScore?.Invoke();
    }

    private void AddMaxCount()
    {
        _maxAmount += 50;
        _currentAmount = 0;
    }
}
