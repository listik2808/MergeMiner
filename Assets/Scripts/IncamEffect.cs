using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class IncamEffect : MonoBehaviour
{
    [SerializeField] private TMP_Text _incum;
    [SerializeField] private float _fadeSpeed;
    [SerializeField] private float _scaleSpeed;
    [SerializeField] private float _moveSpeed;
    [SerializeField] private ParticleSystem _particleSystemIncam;
    [SerializeField] private TimerPartikleUp _partikleUp;
    [SerializeField] private Image _imageCoin;
    [SerializeField] private AnimationCurve _moveCurve;
    [SerializeField] private AnimationCurve _offsetYCurve;

    private Transform _transformPig;

    private void OnEnable()
    {
        _partikleUp.IsPlay += PlayIncamPartikle;
    }

    private void OnDisable()
    {
        _partikleUp.IsPlay -= PlayIncamPartikle;
    }

    public void SetTextId(int id)
    {
        _incum.text = id.ToString();
    }

    public void Init(Transform transform)
    {
        _transformPig = transform;
    }

    private void StartAnimation()
    {
        Sequence sequence = DOTween.Sequence();
        sequence.Insert(0, _incum.DOFade(1, _fadeSpeed));
        sequence.Insert(0, _imageCoin.DOFade(1, _fadeSpeed));
        sequence.Insert(0.1f, _imageCoin.transform.DOMove(_transformPig.position, _moveSpeed).SetEase(_moveCurve));
        sequence.Insert(0.1f, _imageCoin.transform.DOMoveY(_transformPig.position.y, _moveSpeed).SetEase(_offsetYCurve));
        sequence.Insert(0.8f, _incum.DOFade(0, _fadeSpeed));
        sequence.Insert(0.8f, _imageCoin.DOFade(0, _fadeSpeed));
        sequence.Append(_imageCoin.transform.DOLocalMove(Vector3.zero, 0));
    }

    private void PlayIncamPartikle()
    {
        _particleSystemIncam.Play();
        StartAnimation();
    }
}
