using UnityEngine;

namespace Source.Animations.HandPointer
{
    public class HandPointerAnimator : MonoBehaviour
    {
        private const string Click = nameof(Click);
        private const string Drag = nameof(Drag);
        
        [SerializeField] private Animator _animator;

        public void ShowClickAnimation() =>
            _animator.SetTrigger(Click);

        public void ShowDragAnimation() =>
            _animator.SetTrigger(Drag);
    }
}
