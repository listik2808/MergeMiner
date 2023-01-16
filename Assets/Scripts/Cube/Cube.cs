using DG.Tweening;
using Source.Scripts.Infrastructure.Services;
using Source.Scripts.Logic.GoalChecker;
using Source.Scripts.SaveSystem;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Image = UnityEngine.UI.Image;
using Sequence = DG.Tweening.Sequence;

[SelectionBase]
public abstract class Cube : MonoBehaviour
{
    [SerializeField] private int _amountReward;
    [SerializeField] private int _health;
    [SerializeField] private int _damage;
    [SerializeField] private ParticleSystem _particleSystem;
    [SerializeField] private ParticleSystem _particleSystemDistroy;
    [SerializeField] private List<Image> _images;
    [SerializeField] private MeshRenderer _meshRenderer;
    [SerializeField] private BoxCollider _boxCollider;
    [SerializeField] private Canvas _canvas;
    [SerializeField] private TMP_Text _textRewarded;
    [SerializeField] private Image _imageCoin;
    [SerializeField] private float _scaleSpeed;
    [SerializeField] private float _fadeSpeed;
    [SerializeField] private float _moveSpeed;

    private int _starthealth;
    private int _healthShovel;
    private IStorage _storage;

    public int Health => _health;

    private void Awake()
    {
        _storage = AllServices.Container.Single<IStorage>();
        // if(_storage.GetDisplayedLevelNumber() > 10)
        //     _health += Mathf.FloorToInt(_health*5/100);

        _starthealth = _health;
        _imageCoin.gameObject.SetActive(false);
        _textRewarded.text = _amountReward.ToString();
        DisableCollider();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Shovel shovel))
        {
            if (_particleSystem != null)
                _particleSystem.Play();

            shovel.TakeDamage(_damage);
        }
    }

    public void RaiseHealth(int count)
    {
        _health += Mathf.FloorToInt(_health * count / 100);
    }

    public void HealthDecreases()
    {
        _health -= Mathf.FloorToInt(_health * 20 / 100);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out Shovel shovel))
        {
            _particleSystem.Stop();
        }
    }

    public void TakeDamage(int damage)
    {
        _health -= damage;

        if (_health <= 0)
        {
            StartAnimation();
            DeactivateComponents();
            _particleSystemDistroy.Play();
            _health = 0;
            AddSoft();

            if (this is IceCube iceCube)
                iceCube.StartShovel();
        }
        SetPicture();
    }

    public void EnableCollider() =>
        _boxCollider.enabled = true;

    public void DisableCollider() =>
        _boxCollider.enabled = false;

    private void DeactivateComponents()
    {
        DisableCollider();
        _meshRenderer.enabled = false;
        DisableImages();
    }

    public void ActivateComponents()
    {
        _health = _starthealth;
        SetPicture();
        _meshRenderer.enabled = true;

        if(this is IceCube iceCube)
            iceCube.SetShovel();
    }

    private void SetPicture()
    {
        float percentageHealth = (_health * 100) / _starthealth;

        if (percentageHealth <= 30)
        {
            DisableImages();
            _images[2].gameObject.SetActive(true);
            _images[5].gameObject.SetActive(true);
        }
        else if (percentageHealth <= 50)
        {
            DisableImages();
            _images[1].gameObject.SetActive(true);
            _images[4].gameObject.SetActive(true);
        }
        else if (percentageHealth <= 75)
        {
            DisableImages();
            _images[0].gameObject.SetActive(true);
            _images[3].gameObject.SetActive(true);
        }
        else
        {
            DisableImages();
        }

        if (_health == 0)
        {
            DisableImages();
        }
    }

    private void AddSoft()
    {
        int soft = _storage.GetSoft();
        _storage.SetSoft(soft + _amountReward);
        _storage.Save();
    }

    private void DisableImages()
    {
        foreach (var image in _images)
        {
            image.gameObject.SetActive(false);
        }
    }

    private void IncludeImages()
    {
        foreach (var image in _images)
        {
            image.gameObject.SetActive(true);
        }
    }

    private void StartAnimation()
    {
        if(this is IceCube iceCube)
        {
            return;
        }
        else
        {
            _imageCoin.gameObject.SetActive(true);
            Vector3 newVectorUp = new Vector3(0, _imageCoin.transform.position.y + 3, 0);
            Sequence sequence = DOTween.Sequence();
            sequence.Append(_imageCoin.transform.DOScale(1, _scaleSpeed));
            sequence.Insert(0, _imageCoin.DOFade(1, _fadeSpeed));
            sequence.Insert(0.1f, _imageCoin.transform.DOLocalMove(newVectorUp, _moveSpeed));
            sequence.Insert(0.8f, _imageCoin.DOFade(0, _fadeSpeed));
            sequence.Insert(0.8f, _textRewarded.DOFade(0, _fadeSpeed));
            sequence.Append(_imageCoin.transform.DOLocalMove(Vector3.zero, 0));
        }
    }
}
