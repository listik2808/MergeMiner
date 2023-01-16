using System;
using Source.Scripts.Logic.GoalChecker.Boss;
using UnityEngine;

namespace Source.Animations.Chest
{
    public class FinisherChestAnimator : MonoBehaviour, IAnimationStateReader
    {
        private static readonly int Idle = Animator.StringToHash("Idle");
        private static readonly int Hit = Animator.StringToHash("Hit");
        private static readonly int Open = Animator.StringToHash("Open");
        
        private readonly int _isleStateHash = Animator.StringToHash("ChestIdle");
        private readonly int _hitStateHash = Animator.StringToHash("ChestHit");
        private readonly int _openStateHash = Animator.StringToHash("ChestOpen");

        private Animator _animator;

        private void Awake() =>
            _animator = GetComponent<Animator>();

        public void PlayIdle() =>
            _animator.SetTrigger(Idle);

        public void PlayHit() =>
            _animator.SetTrigger(Hit);

        public void PlayOpen() =>
            _animator.SetTrigger(Open);

        public void EnteredState(int stateHash)
        {
            State = StateFor(stateHash);
            StateEntered?.Invoke(State);
        }

        public void ExitedState(int stateHash)
        {
            State = StateFor(stateHash);
            StateExited?.Invoke(State);
        }

        public AnimatorState State { get; private set; }

        public event Action<AnimatorState> StateEntered;

        public event Action<AnimatorState> StateExited;

        private AnimatorState StateFor(int stateHash)
        {
            AnimatorState state;
            if (stateHash == _isleStateHash)
                state = AnimatorState.Idle;
            else if (stateHash == _openStateHash)
                state = AnimatorState.Open;
            else if (stateHash == _hitStateHash)
                state = AnimatorState.Hit;
            else
                state = AnimatorState.Unknown;

            return state;
        }
    }
}