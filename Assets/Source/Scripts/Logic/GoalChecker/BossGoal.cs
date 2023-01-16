using System;
using System.Collections.Generic;
using UnityEngine;

namespace Source.Scripts.Logic.GoalChecker
{
    public class BossGoal : GoalBase
    {
        [SerializeField] private Boss.Boss _boss;
        [SerializeField] private FinisherChest[] _chests;
        [SerializeField] [Min(0)] private int _bossHealth;
        
        public bool IsBossDefeated => _boss.IsDefeated;
        public override IReadOnlyList<FinisherChest> Chests => _chests;
        public override event Action<GoalBase> Checked;

        private void Start() =>
            _boss.Initialize(_bossHealth);

        public override bool CheckWin()
        {
            Checked?.Invoke(this);
            return _boss.IsDefeated;
        }

        public void RaiseHealthBoss(int percent)
        {
            _boss.RaiseHealthBoss(percent);
        }

        public override void ResetGoal()
        {
        }
    }
}
