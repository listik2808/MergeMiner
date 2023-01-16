using Source.Scripts.Infrastructure.Services;
using Source.Scripts.SaveSystem;
using UnityEngine;

public class TestMoneyAdded : MonoBehaviour
{
    [SerializeField] private int _addedSoftAmount;

    private IStorage _storage;

    private void Awake() =>
        _storage = AllServices.Container.Single<IStorage>();

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
            AddSoft();
    }

    public void AddSoft()
    {
        int currentSoft = _storage.GetSoft();
        _storage.SetSoft(currentSoft + _addedSoftAmount);
    }
}