using Source.Scripts.Infrastructure.Factory;
using Source.Scripts.Infrastructure.Services.StaticData;
using Source.Scripts.Logic;
using Source.Scripts.Logic.GoalChecker;
using Source.Scripts.Logic.MainCamera;
using Source.Scripts.SaveSystem;
using UnityEngine;
using Grid = Source.Scripts.Logic.GridBehaviour.Grid;

namespace Source.Scripts.Infrastructure.States
{
    public abstract class GameWorldInitializer
    {
        private readonly IGameFactory _factory;
        private readonly IStorage _storage;
        private readonly IStaticDataService _staticData;

        private const int _light = 25;
        private const int _middle = 45;
        private const int _heavy = 65;
        private const int _unbearable = 85;

        protected GameWorldInitializer(IGameFactory factory, IStorage storage, IStaticDataService staticData)
        {
            _factory = factory;
            _storage = storage;
            _staticData = staticData;
        }
        protected void InitGameWorld()
        {
            RenderSettings.skybox = _staticData.ForLevel(_storage.GetLevel()).Skybox;
            CameraTracking camera = _factory.CreateMainCamera();

            Grid grid = _factory.CreateGrid(new Vector3( 0, 3, 0));
            LevelElements newLevelElements = _factory.CreateLevelElements();
            
            GoalBase goal = _factory.CreateGoal(newLevelElements.GoalPlace.transform.position);

            newLevelElements.RiseHealth(_storage.GetInt(DataNames.VideoAdShownCount) * 10);
            if(goal is BossGoal bossGoal)
                bossGoal.RaiseHealthBoss(_storage.GetInt(DataNames.VideoAdShownCount) * 10);

            if (goal is ChestGoal chestGoal)
            {
                if (chestGoal.TargetCount == 1)
                    newLevelElements.RiseHealth(_light);
                if (chestGoal.TargetCount == 3)
                    newLevelElements.HealthDecreases();
            }

            if (_storage.GetDisplayedLevelNumber() > 35)
            {
                newLevelElements.RiseHealth(_middle);
                if(goal is BossGoal bossGoal1)
                    bossGoal1.RaiseHealthBoss(_middle);            
            }
            else if (_storage.GetDisplayedLevelNumber() > 45)
            {
                newLevelElements.RiseHealth(_heavy);
                if(goal is BossGoal bossGoal1)
                    bossGoal1.RaiseHealthBoss(_heavy);
            }
            else if (_storage.GetDisplayedLevelNumber() > 55)
            {
                newLevelElements.RiseHealth(_unbearable);
                if(goal is BossGoal bossGoal1)
                    bossGoal1.RaiseHealthBoss(_unbearable);
            }

            camera.UI.Construct(grid, _storage, _staticData, goal);
            
            camera.Initialize(grid.CameraDefaultPlace.transform, goal.CameraFinishPlace.transform);

            _factory.UI.AudioButton.Initialize(_storage, _factory);
            if (_storage.GetInt(DataNames.IsMusicPlaying).ToBool())
                _factory.UI.AudioButton.PlayAudio();
            else
                _factory.UI.AudioButton.StopAudio();
        }
    }
}