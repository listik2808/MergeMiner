using System;
using DG.Tweening;
using Shovels;
using Source.Scripts.Logic.Gift;
using Source.Scripts.Logic.GoalChecker.Boss;
using Source.Scripts.Logic.GridBehaviour;
using Source.Scripts.MergedObjects;
using Source.Scripts.SaveSystem;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public abstract class Shovel : MonoBehaviour
{
    [SerializeField] private int _health;
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private int _damage = 1;
    [SerializeField] private float _speed;
    [SerializeField] private Animator _animator;
    [SerializeField] private ParticleSystem _particleSystem;
    
    private IStorage _storage;
    private bool _isAnimationStarted = false;
    protected bool _isOpen = false;
    private GridCell _cell;

    private const string StartRotation = "StartRotation";

    public int Damage => _damage;
    public bool IsOpen => _isOpen;
    public Rigidbody Rigidbody => _rigidbody;

    private void OnTriggerEnter(Collider other)
    {
        if(_rigidbody.isKinematic == false)
        {
            if(other.TryGetComponent(out Cube cube))
            {
                cube.TakeDamage(_damage);

                if (cube.Health > 0)
                {
                    SetSpeed();
                }
            }

            if (other.TryGetComponent(out GiftBox giftBox))
            {
                giftBox.Open();
            }
        
            if(other.TryGetComponent(out Boss boss))
            {
                boss.TakeDamage(_damage);
            }
        }
    }

    private void PlayAnimation()
    {
        _animator.SetTrigger(StartRotation);
    }

    public void SetStorage(IStorage storage)
    {
        _storage = storage;
    }

    public void SetSpeed()
    {
        _rigidbody.velocity = Vector3.zero;
        _rigidbody.AddForce(Vector3.up * _speed, ForceMode.Impulse);
        if (_isAnimationStarted == false)
            PlayAnimation();
    }

    public void TakeDamage(int damage)
    {
        _health -= damage;
        if (_health <= 0)
        {
            if (_particleSystem)
                Instantiate(_particleSystem,transform.position,_particleSystem.transform.rotation).Play();

            _health = 0;
            gameObject.SetActive(false);
        }
    }

    public virtual void NewItemDiscovered() { }
}