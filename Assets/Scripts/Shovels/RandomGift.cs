using System;
using System.Collections;
using Source.Scripts.Infrastructure.Services;
using Source.Scripts.Logic.Cell;
using Source.Scripts.Logic.GridBehaviour;
using Source.Scripts.SaveSystem;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

[SelectionBase]
public class RandomGift : CellContent
{
    private const int MinShovelId = 1;
    
    [SerializeField] private Button _boxRandomShovels;
    [SerializeField] private ParticleSystem _particleSystem;
    [SerializeField] private Transform _model;
    [SerializeField] private AnimationCurve _appearScaleCurve;
    [SerializeField] [Min(0.01f)] private float _appearDuration = 0.2f;
    [SerializeField] [Min(1)] private int _shovelsIdRange = 3;
    [SerializeField] [Min(0)] private int _shovelsDegradeOffset = 5;

    private IStorage _storage;
    
    public event Action<GridCell, int> Activated;

    public void Construct(IStorage storage)
    {
        _storage = storage;
    }

    public void Enable() =>
        enabled = true;
    
    public void Disable() =>
        enabled = false;

    public void ShowAppear()
    {
        StartCoroutine(Appear());
    }

    private void OnEnable()
    {
        _boxRandomShovels.onClick.AddListener(OnButtonClick);
    }

    private void OnDisable()
    {
        _boxRandomShovels.onClick.RemoveListener(OnButtonClick);
    }

    private void OnButtonClick()
    {
        int maxMergedShovelLevel = _storage.GetInt(DataNames.MaxMergedShovelLevel);
        
        int id = Random.Range(0, _shovelsIdRange);
        id += maxMergedShovelLevel - _shovelsDegradeOffset;
        id = Math.Clamp(id, MinShovelId, maxMergedShovelLevel);

        if (_particleSystem)
        {
            _particleSystem.transform.parent = null;
            _particleSystem.Play();
        }
        
        Activated?.Invoke(ParentCell, id);
    }
    
    private IEnumerator Appear()
    {
        for (float t = 0; t < 1; t += Time.deltaTime / _appearDuration)
        {
            _model.localScale = Vector3.one * _appearScaleCurve.Evaluate(t);
            yield return null;
        }

        _model.localScale = Vector3.one;
    }
}
