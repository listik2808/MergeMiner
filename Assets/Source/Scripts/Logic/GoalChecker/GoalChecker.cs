using System.Collections.Generic;
using System.Linq;
using Source.Scripts.Infrastructure.States;
using Source.Scripts.MergedObjects;
using UnityEngine;

namespace Source.Scripts.Logic.GoalChecker
{
    public class GoalChecker : MonoBehaviour
    {
        private IGameStateMachine _stateMachine;
        private IReadOnlyList<MergebleObject> _shovels;
        private GoalBase _goal;

        private List<MergebleObject> _activeShovels = new List<MergebleObject>();

        public GoalBase Goal => _goal;

        private void Awake() =>
            Disable();

        public void Initialize(IGameStateMachine stateMachine, IReadOnlyList<MergebleObject> shovels, GoalBase goal)
        {
            _stateMachine = stateMachine;
            _shovels = shovels;
            _goal = goal;
        }

        private void FixedUpdate()
        {
            _goal.CheckWin();
            
            _activeShovels = _shovels.Where(s => s.gameObject.activeInHierarchy).ToList();
            if (_activeShovels.Count == 0)
            {
                Disable();
                SetWinOrFail();
            }
        }

        public void Enable() =>
            enabled = true;

        public void Disable() =>
            enabled = false;

        private void SetWinOrFail()
        {
            if (_goal.CheckWin())
            {
                
                _stateMachine.Enter<FinishLevelState, IReadOnlyList<int>>(CreateChestRewardList());
            }
            else
            {
                _stateMachine.Enter<FailState>();
            }
        }

        private List<int> CreateChestRewardList()
        {
            return _goal.Chests.Where(ch => ch.IsHit).Select(ch => ch.RewardId).ToList();
        }
    }
}
