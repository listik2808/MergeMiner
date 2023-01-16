using Source.Scripts.UI.Slider;
using UnityEngine;

namespace Source.Scripts.Logic.GoalChecker.Boss
{
    [RequireComponent(typeof(Collider))]
    public class Boss : MonoBehaviour
    {
        [SerializeField] private GolemAnimator _golemAnimator;
        [SerializeField] private UISlider _healthBar;
        [SerializeField] private int _damage = 1;
        [SerializeField] private Collider _selfCollider;
        
        private int _maxHealth = 10;
        private int _currentHealth;
        
        public bool IsDefeated { get; private set; }

        private void Awake()
        {
            _golemAnimator.StateEntered += OnStateEntered;
            _golemAnimator.StateExited += OnStateExited;
        }

        public void Initialize(int maxHealth)
        {
            _maxHealth = maxHealth;
            _currentHealth = _maxHealth;
            _healthBar.SetValue(_currentHealth, _maxHealth);
        }

        private void OnTriggerEnter(Collider other)
        {
            Shovel shovel = other.GetComponent<Shovel>();

            if (shovel)
            {
                shovel.TakeDamage(_damage);
                shovel.SetSpeed();
                _golemAnimator.PlayTakeDamage();
            }
        }

        public void RaiseHealthBoss(int percent)
        {
            _maxHealth += Mathf.FloorToInt(_maxHealth * percent / 100);
            Initialize(_maxHealth);
        }

        private void OnStateEntered(AnimatorState state)
        {
            if (state == AnimatorState.Die)
            {
                _golemAnimator.StateEntered -= OnStateEntered;
                _healthBar.gameObject.SetActive(false);
            }
        }

        private void OnStateExited(AnimatorState state)
        {
            if (state == AnimatorState.Die)
            {
                _golemAnimator.StateExited -= OnStateExited;
                gameObject.SetActive(false);
            }
        }

        public void TakeDamage(int damage)
        {
            _currentHealth -= damage;
            if (_currentHealth <= 0)
            {
                _currentHealth = 0;
                _selfCollider.enabled = false;
                _golemAnimator.PlayDie();
                IsDefeated = true;
            }

            _healthBar.SetValue(_currentHealth, _maxHealth);
        }
    }
}