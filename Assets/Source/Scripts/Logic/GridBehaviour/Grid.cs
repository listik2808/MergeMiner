using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Source.Scripts.Infrastructure.Factory;
using Source.Scripts.Logic.Cell;
using Source.Scripts.Logic.MainCamera;
using Source.Scripts.Logic.Monetization;
using Source.Scripts.MergedObjects;
using Source.Scripts.SaveSystem;
using Source.Scripts.StaticData;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Source.Scripts.Logic.GridBehaviour
{
    public class Grid : MonoBehaviour
    {
        private const int Empty = 0;
        
        [SerializeField] private Transform _content;
        [SerializeField] private CameraDefaultPlace _cameraDefaultPlace;
        [SerializeField] private TrashZone _trashZone;
        
        private List<GridCell> _cells = new List<GridCell>();
        private List<MergebleObject> _shovels = new List<MergebleObject>();
        private IGameFactory _factory;
        private IStorage _storage;
        private CameraTracking _cameraTracking;
        private CellContentInfo _cellContentInfo;

        public CameraDefaultPlace CameraDefaultPlace => _cameraDefaultPlace;
        public IReadOnlyList<MergebleObject> Shovels => _shovels;
        
        public event Action<int> ShovelSpawned;

        public void Initialize(IGameFactory factory, IStorage storage, int size, CellContentInfo cellContentInfo)
        {
            _factory = factory;
            _storage = storage;
            _cellContentInfo = cellContentInfo;
            
            if(_storage.GetInt(DataNames.IsTutorialComplete).ToBool() == false)
                _trashZone.gameObject.SetActive(false);
            
            SpawnCells(count: size);

            StartCoroutine(DelayedLoadProgress());
        }

        public void AddShovel(MergebleObject shovel)
        {
            shovel.StopInteractable();
            _shovels.Add(shovel);
        }

        public CellContent AddContent(int id)
        {
            GridCell cell = GetRandomCell();
            if (cell)
            {
                CellContent content = SpawnContentBy(id, cell);
                switch (content)
                {
                    case MergebleObject shovel:
                        shovel.StopInteractable();
                        break;
                    case RandomGift gift:
                        gift.Disable();
                        break;
                }

                return content;
            }

            return null;
        }

        public void DropShovels()
        {
            foreach (var cell in _cells.Where(cell => cell.CellContent is AdvGift))
                RemoveContentFrom(cell);
            
            UpdateProgress();

            foreach (var shovel in _shovels)
            {
                shovel.Shovel.Rigidbody.isKinematic = false;
            }
        }

        public void EnableGridContent()
        {
            foreach (GridCell cell in _cells)
                switch (cell.CellContent)
                {
                    case MergebleObject shovel:
                        shovel.StartInteractable();
                        break;
                    case RandomGift gift:
                        gift.Enable();
                        break;
                    case AdvGift advGift:
                        advGift.Enable();
                        break;
                }
        }

        public void DisableGridContent()
        {
            foreach (GridCell cell in _cells)
                switch (cell.CellContent)
                {
                    case MergebleObject shovel:
                        shovel.StopInteractable();
                        break;
                    case RandomGift gift:
                        gift.Disable();
                        break;
                    case AdvGift advGift:
                        advGift.Disable();
                        break;
                }
        }

        public void RespawnGridContent()
        {
            foreach (var shovel in _shovels)
                if(shovel != null)
                    Destroy(shovel.gameObject);
            _shovels.Clear();
            
            foreach (var cell in _cells)
                if (cell.CellContent != null)
                {
                    Destroy(cell.CellContent.gameObject);
                    cell.RemoveObject();
                }

            LoadProgress();
        }

        public void UpdateProgress()
        {
            for (int i = 0; i < _cells.Count; i++)
            {
                if (_cells[i].IsOccupied)
                    _storage.SetInt(DataNames.CellContent + i, _cells[i].CellContent.ID);
                else
                    _storage.SetInt(DataNames.CellContent + i, Empty);
            }
            _storage.Save();
        }

        public void TryBuyShovel()
        {
            int soft = _storage.GetSoft();
            var shovelConfig = _cellContentInfo.ShovelConfigs[_storage.GetInt(DataNames.CurrentShovelLevel) - 1];
            
            int price = shovelConfig.CellContentPrefab.GetComponent<MergebleObject>() ? shovelConfig.CellContentPrefab.GetComponent<MergebleObject>().Price : 100;
            if(soft < price) 
                return;

            GridCell cell = GetRandomCell();
            if(cell == null)
                return;
                
            SpawnContentBy(_storage.GetInt(DataNames.CurrentShovelLevel), GetRandomCell());
            
            _storage.SetSoft(soft - price);
            _storage.SetInt(DataNames.PurchasedShovelsNumber, _storage.GetInt(DataNames.PurchasedShovelsNumber) + 1);
            
            if (_storage.GetInt(DataNames.CurrentShovelLevel) < _cellContentInfo.ShovelCountsToNextPurchaseLevel.Length && _storage.GetInt(DataNames.PurchasedShovelsNumber) >=
                _cellContentInfo.ShovelCountsToNextPurchaseLevel[_storage.GetInt(DataNames.CurrentShovelLevel)])
            {
                _storage.SetInt(DataNames.PurchasedShovelsNumber, 0);
                _storage.SetInt(DataNames.CurrentShovelLevel, _storage.GetInt(DataNames.CurrentShovelLevel) + 1);
            }

            UpdateProgress();
        }

        private void LoadProgress()
        {
            for (int i = 0; i < _cells.Count; i++)
            {
                if (_storage.HasKeyInt(DataNames.CellContent + i))
                {
                    int cellContentId = _storage.GetInt(DataNames.CellContent + i);
                    if(cellContentId != 0)
                        SpawnContentBy(cellContentId, _cells[i]);
                }
            }
        }

        private CellContent SpawnContentBy(int id, GridCell cell)
        {
            float zOffset = -0.5f;
            Vector3 contentPosition = new Vector3(cell.transform.position.x, cell.transform.position.y,
                cell.transform.position.z + zOffset);

            CellContent newCellContent = _factory.CreateCellContent(id, contentPosition);
            newCellContent.SetParentCell(cell);
            cell.AddObject(newCellContent);
            switch (newCellContent)
            {
                case RandomGift randomGift:
                    randomGift.Activated += (cell, id) => OnGiftActivated(cell, id);
                    randomGift.ShowAppear();
                    break;
                case AdvGift advGift:
                    advGift.Activated += (cell, id) => OnAdvGiftActivated(cell, id);
                    advGift.Disappear += (cell) => OnAdvDisappear(cell);
                    break;
                case MergebleObject mergebleObject:
                    _shovels.Add(mergebleObject);
                    mergebleObject.Merging += (targetCell, selfCell, id) => OnMerging(targetCell, selfCell, id);
                    mergebleObject.Removed += (selfcel) => RemoveContentFrom(selfcel);
                    mergebleObject.Highlighting += (id, isEnable) => OnHighlightFor(id, isEnable);
                    mergebleObject.ShowAppear();
                    ShovelSpawned?.Invoke(id);
                    break;
            }

            return newCellContent;
        }

        private void OnAdvDisappear(GridCell cell)
        {
            RemoveContentFrom(cell);
        }

        private void OnAdvGiftActivated(GridCell cell, int id)
        {
            RemoveContentFrom(cell);
            SpawnContentBy(id, cell);
            UpdateProgress();
        }

        private void OnMerging(GridCell targetCell, GridCell selfCell, int id)
        {
            targetCell.Collider.enabled = false;
            selfCell.Collider.enabled = false;
            
            if(targetCell.CellContent is MergebleObject target)
                target.ShowDisappear(true, () =>
                {
                    RemoveContentFrom(targetCell);
                    CellContent content = SpawnContentBy(id, targetCell);
                    if (content is MergebleObject mergebleObject)
                       mergebleObject.MergeParticles.Play();

                    targetCell.Collider.enabled = true;
                    selfCell.Collider.enabled = true;
                });
            
            if(selfCell.CellContent is MergebleObject self)
                self.ShowDisappear(false, () => RemoveContentFrom(selfCell));
            
            UpdateProgress();
        }

        private void OnGiftActivated(GridCell cell, int id)
        {
            RemoveContentFrom(cell);
            SpawnContentBy(id, cell);
            UpdateProgress();
        }

        private void OnHighlightFor(int id, bool enable)
        {
            foreach (var cell in _cells)
                if (cell.CellContent is MergebleObject mergebleObject)
                    if(mergebleObject.ID == id)
                        mergebleObject.Highlight(enable);
        }

        private void RemoveContentFrom(GridCell cell)
        {
            CellContent cellContent = cell.CellContent;
            cell.RemoveObject();
            if (cellContent is MergebleObject mergebleObject)
                _shovels.Remove(mergebleObject);
            Destroy(cellContent.gameObject);
        }

        private void SpawnCells(int count)
        {
            for (int i = 0; i < count; i++)
            {
                GridCell gridCell = _factory.CreateCell(_content);
                _cells.Add(gridCell);
            }
        }

        private GridCell GetRandomCell()
        {
            List<GridCell> emptyCells = _cells.Where(c => c.IsOccupied == false).ToList();
            
            return emptyCells.Count > 0 ? emptyCells[Random.Range(0, emptyCells.Count)] : null;
        }

        private IEnumerator DelayedLoadProgress()
        {
            yield return new WaitForEndOfFrame();

            LoadProgress();
        }
    }
}