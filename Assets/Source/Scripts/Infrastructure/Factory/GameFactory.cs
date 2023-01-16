using System;
using Source.Scripts.Infrastructure.AssetManagement;
using Source.Scripts.Infrastructure.Services.StaticData;
using Source.Scripts.Infrastructure.States;
using Source.Scripts.Logic;
using Source.Scripts.Logic.Cell;
using Source.Scripts.Logic.GoalChecker;
using Source.Scripts.Logic.GridBehaviour;
using Source.Scripts.Logic.MainCamera;
using Source.Scripts.Logic.Monetization;
using Source.Scripts.MergedObjects;
using Source.Scripts.SaveSystem;
using Source.Scripts.StaticData;
using UnityEngine;
using Grid = Source.Scripts.Logic.GridBehaviour.Grid;
using Object = UnityEngine.Object;

namespace Source.Scripts.Infrastructure.Factory
{
    public class GameFactory : IGameFactory
    {
        private readonly IAssetProvider _assetProvider;
        private readonly IStorage _storage;
        private readonly IStaticDataService _staticData;
        private readonly IGameStateMachine _stateMachine;

        public Grid Grid { get; private set; }
        public GoalChecker GoalChecker { get; private set; }
        public UI.UI UI { get; private set; }
        public LevelElements LevelElements { get; private set; }
        public CameraTracking CameraTracking { get; private set; }
        public AudioObject AudioObject { get; private set; }

        public event Action GridCreated;

        public GameFactory(IAssetProvider assetProvider, IStorage storage, IStaticDataService staticData, IGameStateMachine stateMachine)
        {
            _assetProvider = assetProvider;
            _storage = storage;
            _staticData = staticData;
            _stateMachine = stateMachine;
        }

        public Grid CreateGrid(Vector3 at)
        {
            LevelConfig _levelConfig = _staticData.ForLevel(_storage.GetLevel());
            CellContentInfo cellContentInfo = _staticData.ForShovelInfo();
            Grid = _assetProvider.Instantiate<Grid>(AssetPath.GridPath, at);
            Grid.Initialize(this, _storage, _levelConfig.CellCount, cellContentInfo);

            GridCreated?.Invoke();
            return Grid;
        }

        public LevelElements CreateLevelElements() => 
            LevelElements = _assetProvider.Instantiate<LevelElements>(_staticData.ForLevel(_storage.GetLevel()).LevelElementsPath);

        public AudioObject CreateAudio() => 
            AudioObject = _assetProvider.Instantiate<AudioObject>(AssetPath.AudioObjectPath);

        public GoalBase CreateGoal(Vector3 at)
        {
            GoalBase goal = _assetProvider.Instantiate<GoalBase>(_staticData.ForLevel(_storage.GetLevel()).GoalPath, at);
            goal.SetChestsReward(_staticData.ForLevel(_storage.GetLevel()).ChestReward);
            
            GoalChecker = goal.GetComponent<GoalChecker>();
            GoalChecker.Initialize(_stateMachine, Grid.Shovels, goal);
            
            return goal;
        }

        public CameraTracking CreateMainCamera()
        {
            CameraTracking camera = _assetProvider.Instantiate<CameraTracking>(AssetPath.CameraPath);

            UI = camera.UI;
            CameraTracking = camera;

            return camera;
        }

        public GridCell CreateCell(Transform parent) => 
            _assetProvider.Instantiate<GridCell>(AssetPath.CellPath, parent);

        public CellContent CreateCellContent(int id, Vector3 at)
        {
            CellContent prefab = _staticData.ForCellContent(id).CellContentPrefab;
            CellContent newCellContent = Object.Instantiate(prefab, at, Quaternion.identity);
            
            switch (newCellContent)
            {
                case MergebleObject mergebleObject:
                    mergebleObject.Initialize(_staticData);
                    mergebleObject.Shovel.SetStorage(_storage);
                    mergebleObject.GetComponent<IncamEffect>().Init(UI.MergeMenu.OfflineRewardView.PigPoint);
                    break;
                case RandomGift randomGift:
                    randomGift.Construct(_storage);
                    break;
                case AdvGift advGift:
                    advGift.Construct(_staticData, UI);
                    break;
            }

            return newCellContent;
        }
    }
}
