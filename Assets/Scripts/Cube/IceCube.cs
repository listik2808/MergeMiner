using System.Linq;
using Source.Scripts.Infrastructure.Factory;
using Source.Scripts.Infrastructure.Services;
using Source.Scripts.MergedObjects;
using UnityEngine;
using Grid = Source.Scripts.Logic.GridBehaviour.Grid;

public class IceCube : Cube
{
    private IGameFactory _factory;
    private Grid _grid;
    private MergebleObject _mergebleObject;

    private void Start()
    {
        _factory = AllServices.Container.Single<IGameFactory>();
        if (_factory.Grid == null)
            _factory.GridCreated += OnGridCreated;
        else
            _grid = _factory.Grid;

        _mergebleObject = GetComponentsInChildren<MergebleObject>().FirstOrDefault(mergeable => mergeable.isActiveAndEnabled);

        if(_mergebleObject != null )
            _mergebleObject.StopInteractable();
    }

    private void OnGridCreated()
    {
        _factory.GridCreated -= OnGridCreated;
        _grid = _factory.Grid;
    }

    public void StartShovel()
    {
        var newMergeableObject = Instantiate(_mergebleObject, transform.position, Quaternion.identity);
        _grid.AddShovel(newMergeableObject);

        newMergeableObject.Shovel.Rigidbody.isKinematic = false;
        newMergeableObject.Shovel.GetComponent<SphereCollider>().enabled = true;
        _mergebleObject.gameObject.SetActive(false);
    }

    public void SetShovel()
    {
        if (_mergebleObject != null)
            _mergebleObject.gameObject.SetActive(true);
    }
}
