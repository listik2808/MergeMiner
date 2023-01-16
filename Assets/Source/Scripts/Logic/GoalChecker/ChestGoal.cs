using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Source.Scripts.Logic.GoalChecker
{
    public class ChestGoal : GoalBase
    {
        [SerializeField] [Min(0)] private int _targetCount;
        [SerializeField] private FinisherChest[] _chests;

        public int TargetCount => _targetCount;
        public int CurrentOpenedChestCount { get; private set; }

        public override IReadOnlyList<FinisherChest> Chests => _chests;
        public override event Action<GoalBase> Checked;

        public override bool CheckWin()
        {
            var openedChests = _chests.Where(c => c.IsHit).ToList();
            CurrentOpenedChestCount = openedChests.Count;
            
            if (CurrentOpenedChestCount >= _targetCount)
            {
                foreach (var chest in openedChests)
                    chest.Animator.PlayOpen();
                Checked?.Invoke(this);
                return true;
            }
            Checked?.Invoke(this);
            return false;
        }

        public override void ResetGoal()
        {
            foreach (var chest in _chests)
            {
                CurrentOpenedChestCount = 0;
                chest.ResetHit();
                chest.Animator.PlayIdle();
            }
            Checked?.Invoke(this);
        }
    }
}