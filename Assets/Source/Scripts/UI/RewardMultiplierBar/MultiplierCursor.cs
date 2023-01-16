using System;
using UnityEngine;

namespace Source.Scripts.UI.RewardMultiplierBar
{
    public class MultiplierCursor : MonoBehaviour
    {
        [SerializeField] private RectTransform _rect;
        [SerializeField] private float _lenght = 1;
        [SerializeField] private float _xPosition;
        [SerializeField] private float _moveSpeed = 1;
    
        private float _currentXPosition;
        private bool _isMoving = true;
        public event Action<int>MultiplierUpdated;
    
        private void Update()
        {
            if (_isMoving)
            {
                var value = Mathf.PingPong(Time.time * _moveSpeed, _lenght);
                UpdatePosition(value);
            }
        }

        private void UpdatePosition(float value)
        {
            var targetPosition = Mathf.Lerp(-_xPosition, _xPosition, value);
            _rect.transform.localPosition = new Vector3(targetPosition, _rect.transform.localPosition.y,
                _rect.transform.localPosition.z);
        }

        private void OnTriggerEnter(Collider other)
        {
            if(other.TryGetComponent(out MultiplierValue multiplierValue))
                MultiplierUpdated?.Invoke(multiplierValue.Value);
        }

        public void Stop()
        {
            _isMoving = false;
        }
    }
}
