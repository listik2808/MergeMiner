using System;
using System.Collections.Generic;
using Source.Scripts.Logic.MainCamera;
using UnityEngine;

namespace Source.Scripts.Logic.GoalChecker
{
    public abstract class GoalBase : MonoBehaviour
    {
        [SerializeField] private Sprite _goalIcon;
        [SerializeField] private CameraFinishPlace _cameraFinishPlace;
        
        public Sprite GoalIcon => _goalIcon;
        public CameraFinishPlace CameraFinishPlace => _cameraFinishPlace;
        public abstract IReadOnlyList<FinisherChest> Chests { get; }

        public abstract event Action<GoalBase> Checked;

        public abstract bool CheckWin();
        public abstract void ResetGoal();

        public void SetChestsReward(IReadOnlyList<int> rewardIds)
        {
            for (int i = 0; i < Chests.Count; i++)
            {
                if(i < rewardIds.Count)
                    Chests[i].SetReward(rewardIds[i]);
            }
        }
    }
}