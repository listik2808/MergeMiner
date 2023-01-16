using System;
using System.Collections;
using Shovels;
using Source.Scripts.Infrastructure.Services.StaticData;
using Source.Scripts.Logic.Cell;
using Source.Scripts.Logic.GridBehaviour;
using TMPro;
using UnityEngine;

namespace Source.Scripts.MergedObjects
{
    [SelectionBase]
    public class MergebleObject : CellContent
    {
        private const float ZOffset = -0.5f;
        private const float GrabZOffset = -0.9f;

        [SerializeField] private ParticleSystem _mergeParticles;
        [SerializeField] private Transform _model;
        [SerializeField] private AnimationCurve _appearScaleCurve;
        [SerializeField] private AnimationCurve _disappearPositionCurve;
        [SerializeField] private AnimationCurve _disappearScaleCurve;
        [SerializeField][Min(0.01f)] private float _appearDuration = 0.2f;
        [SerializeField][Min(0.01f)] private float _disappearDuration = 0.8f;
        [SerializeField] private Highlighter _highlighter;
        [SerializeField] private TMP_Text _gradeText;
        [SerializeField] private Shovel _shovel;
        [SerializeField] private int _price;
        [SerializeField] private TimerPartikleUp _timerPartikleUp;
        [SerializeField] private IncamEffect _incamEffect;

        private GridCell _targetCell;
        private IStaticDataService _staticData;
        private Camera _camera;
        private Plane _plane;
        private bool _canDestroy;
        private bool _isInteractable = true;

        public ParticleSystem MergeParticles => _mergeParticles;
        public Shovel Shovel => _shovel;
        public int Price => _price;

        public event Action<GridCell, GridCell, int> Merging;
        public event Action<int, bool> Highlighting; 
        public event Action<GridCell> Removed;

        private void Awake()
        {
            _camera = Camera.main;
            _plane = new Plane(Vector3.back, transform.position);
            _gradeText.text = ID.ToString();
            _isInteractable = false;
            _incamEffect.SetTextId(ID);
        }

        public void Initialize(IStaticDataService staticData) =>
            _staticData = staticData;

        public void ShowAppear()
        {
            StartCoroutine(Appear());
        }

        public void ShowDisappear(bool isLeft, Action callback)
        {
            StartCoroutine(Disappear(isLeft, callback));
        }

        public void StartInteractable()
        {
            _isInteractable = true;
            _gradeText.gameObject.SetActive(true);
            _timerPartikleUp.StartTimer();
        }
        
        public void StopInteractable()
        {
            _isInteractable = false;
            if(ParentCell != null)
                transform.position = ParentCell.transform.position + new Vector3( 0, 0, ZOffset);
            _gradeText.gameObject.SetActive(false);
            _timerPartikleUp.StopTimer();
        }

        public void Highlight(bool isEnable)
        {
            if(isEnable)
                _highlighter.HighlightOn();
            else
                _highlighter.HighlightOff();
        }
        
        private void OnMouseDown()
        {
            if(_isInteractable == false)
                return;
            
            Highlighting?.Invoke(ID, true);
        }

        private void OnMouseDrag()
        {
            if(_isInteractable == false)
                return;

            transform.position = GetMouseWorldPosition() + Vector3.forward * GrabZOffset;
        }

        private Vector3 GetMouseWorldPosition()
        {
            Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
            if (_plane.Raycast(ray, out float distance))
                return ray.GetPoint(distance);
            else
                return transform.position;
        }

        private void OnMouseUp()
        {
            if(_isInteractable == false)
                return;

            if (_targetCell)
            {
                if (_targetCell.CellContent)
                {
                    if (_targetCell.CellContent.ID == ID && _staticData.ForCellContent(ID + 1) != null)
                    {
                        transform.position = _targetCell.transform.position + new Vector3(0, 0, ZOffset);
                        Merging?.Invoke(_targetCell, ParentCell, ID + 1);
                    }
                    else
                    {
                        _targetCell.CellContent.transform.position = ParentCell.transform.position + new Vector3( 0, 0, ZOffset);
                        ParentCell.AddObject(_targetCell.CellContent);
                        _targetCell.CellContent.SetParentCell(ParentCell);
                        
                        transform.position = _targetCell.transform.position + new Vector3( 0, 0, ZOffset);
                        _targetCell.AddObject(this);
                        SetParentCell(_targetCell);
                    }
                }
                else
                {
                    ParentCell.RemoveObject();
                    transform.position = _targetCell.transform.position + new Vector3( 0, 0, ZOffset);
                    _targetCell.AddObject(this);
                    SetParentCell(_targetCell);
                }
            }
            else
            {
                transform.position = ParentCell.transform.position + new Vector3( 0, 0, ZOffset);
            }

            _targetCell = null;
            
            Highlighting?.Invoke(ID, false);

            if (_canDestroy)
            {
                Removed?.Invoke(ParentCell);
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out GridCell gridCell) && gridCell != ParentCell)
                _targetCell = gridCell;
            
            if (other.TryGetComponent(out TrashZone trashZone)) 
                _canDestroy = true;
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.TryGetComponent(out GridCell gridCell) && gridCell == _targetCell)
                _targetCell = null;
            
            if (other.TryGetComponent(out TrashZone trashZone)) 
                _canDestroy = false;
        }

        private IEnumerator Appear()
        {
            StopInteractable();
            for (float t = 0; t < 1; t += Time.deltaTime / _appearDuration)
            {
                _model.localScale = Vector3.one * _appearScaleCurve.Evaluate(t);
                yield return null;
            }

            _model.localScale = Vector3.one;
            StartInteractable();
        }

        private IEnumerator Disappear(bool isLeft, Action callback)
        {
            _isInteractable = false;
            _gradeText.gameObject.SetActive(false);
            
            for (float t = 0; t < 1; t += Time.deltaTime / _disappearDuration)
            {
                _model.localScale = Vector3.one * _disappearScaleCurve.Evaluate(t);
                if(isLeft)
                    _model.localPosition = -Vector3.right * _disappearPositionCurve.Evaluate(t);
                else
                    _model.localPosition = Vector3.right * _disappearPositionCurve.Evaluate(t);

                yield return null;
            }
            callback?.Invoke();
        }
    }
}