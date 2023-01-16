using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    [SerializeField] private float _positionX;
    [SerializeField] private float _maxDelta;
    [SerializeField] private GameObject _gameObject;
    [SerializeField] private float _timeDelay;
    [SerializeField] private ParticleSystem _particleSystem;

    private Coroutine _bombaMovemebt;
    private float _startPositionX;
    //private bool _reachedGoal = false;
    private int _damage = 999999;
    private float _elepsedTime = 0;

    private void Start()
    {
        _startPositionX = transform.position.x;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Shovel shovel))
        {
            SartMovementCorutine();
        }

        if (other.TryGetComponent(out Cube cube))
        {
            cube.TakeDamage(_damage);
        }
    }

    public void TurnOffBomb()
    {
        //_reachedGoal = true;
        gameObject.SetActive(false);
    }

    private IEnumerator Movements()
    {

        while (_elepsedTime < _timeDelay)
        {
            _elepsedTime += Time.deltaTime;
            _startPositionX = Mathf.MoveTowards(_startPositionX, _positionX, _maxDelta);
            transform.position = new Vector3(_startPositionX, transform.position.y, transform.position.z);
            yield return null;
        }

        _elepsedTime = 0;
        gameObject.SetActive(false);
    }

    private void SartMovementCorutine()
    {
        _gameObject.gameObject.SetActive(false);

        if (_particleSystem != null)
            _particleSystem.Play();

        if (_bombaMovemebt != null)
            StopCoroutine(_bombaMovemebt);

        _bombaMovemebt = StartCoroutine(Movements());
    }
}
