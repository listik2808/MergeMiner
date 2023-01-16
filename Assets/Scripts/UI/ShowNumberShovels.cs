using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShowNumberShovels : MonoBehaviour
{
    [SerializeField] private TMP_Text _textCount;
    [SerializeField] private TMP_Text _level;
    [SerializeField] private ShovelsPurchaseCounter _purchaseCounter;

    private void OnEnable()
    {
        _purchaseCounter.ShowedScore += Show;
    }

    private void OnDisable()
    {
        _purchaseCounter.ShowedScore -= Show;
    }

    private void Start()
    {
        Show();
    }

    private void Show()
    {
        _textCount.text = _purchaseCounter.CurrentAmount.ToString();
        _level.text = _purchaseCounter.CurrentShovelLevel+1.ToString();
    }
}
