using System;
using DG.Tweening;
using TMPro;
using UnityEngine;

namespace Shovels
{
    public class IncomeMoneyDisplay : MonoBehaviour
    {
        [SerializeField] private Shovel _shovel;
        [SerializeField] private TMP_Text _incomeText;
        [SerializeField] private float _fadeSpeed = 0.3f;
        [SerializeField] private float _moveSpeed = 1;
        [SerializeField] private float _scaleSpeed = 0.2f;

        private Sequence _sequence;
        
        private void Awake()
        {
            _incomeText.alpha = 0;
            //_incomeText.text = _shovel.MoneyIncome.ToString();
        }

        //private void OnEnable() =>
        //    _shovel.TimerCompleted += OnCompleted;

        //private void OnDisable() =>
        //    _shovel.TimerCompleted -= OnCompleted;

        private void OnDestroy() =>
            _sequence.Kill();

        private void OnCompleted() =>
            StartAnimation();

        private void StartAnimation()
        {
            _sequence = DOTween.Sequence();
            _sequence.Append(_incomeText.transform.DOScale(1, _scaleSpeed));
            _sequence.Insert(0,_incomeText.DOFade(1, _fadeSpeed));
            _sequence.Insert(0.1f, _incomeText.transform.DOLocalMove(Vector3.up, _moveSpeed));
            _sequence.Insert(0.8f, _incomeText.DOFade(0, _fadeSpeed));
            _sequence.Append(_incomeText.transform.DOLocalMove(Vector3.zero, 0));
        }
    }
}