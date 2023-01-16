using System;
using Source.Scripts.Infrastructure.Services;
using Source.Scripts.Logic;
using Source.Scripts.Logic.Cell;
using Source.Scripts.Logic.GoalChecker;
using Source.Scripts.Logic.GridBehaviour;
using Source.Scripts.Logic.MainCamera;
using UnityEngine;
using Grid = Source.Scripts.Logic.GridBehaviour.Grid;

namespace Source.Scripts.Infrastructure.Factory
{
    public interface IGameFactory : IService
    {
        Grid Grid { get; }
        GoalChecker GoalChecker { get; }
        UI.UI UI { get; }
        LevelElements LevelElements { get; }
        CameraTracking CameraTracking { get; }
        AudioObject AudioObject { get; }

        event Action GridCreated;
        
        Grid CreateGrid(Vector3 at);
        GridCell CreateCell(Transform parent);
        GoalBase CreateGoal(Vector3 at);
        LevelElements CreateLevelElements();
        CellContent CreateCellContent(int id, Vector3 at);
        CameraTracking CreateMainCamera();
        AudioObject CreateAudio();
    }
}