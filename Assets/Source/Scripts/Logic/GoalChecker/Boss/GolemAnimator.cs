using System;
using UnityEngine;

namespace Source.Scripts.Logic.GoalChecker.Boss
{
    [RequireComponent(typeof(Animator))]
    public class GolemAnimator : MonoBehaviour, IAnimationStateReader
    {
        private static readonly int Idle = Animator.StringToHash("Idle");
        private static readonly int TakeDamage = Animator.StringToHash("TakeDamage");
        private static readonly int Die = Animator.StringToHash("Die");
        
        private readonly int _dieStateHash = Animator.StringToHash("Die");
        private readonly int _takeDamageStateHash = Animator.StringToHash("Take Damage");
        private readonly int _isleStateHash = Animator.StringToHash("Idle");

        private Animator _animator;

        private void Awake() =>
            _animator = GetComponent<Animator>();

        public void PlayDie() =>
            _animator.SetTrigger(Die);

        public void PlayTakeDamage() =>
            _animator.SetTrigger(TakeDamage);

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
            else if (stateHash == _dieStateHash)
                state = AnimatorState.Die;
            else if (stateHash == _takeDamageStateHash)
                state = AnimatorState.TakeDamage;
            else
                state = AnimatorState.Unknown;

            return state;
        }
    }
}