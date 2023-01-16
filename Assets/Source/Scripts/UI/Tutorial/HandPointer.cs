using System;
using System.Collections;
using Source.Animations.HandPointer;
using UnityEngine;

namespace Source.Scripts.UI.Tutorial
{
    public class HandPointer : MonoBehaviour
    {
        [SerializeField] private RectTransform _rectTransform;
        [SerializeField] private HandPointerAnimator _handPointerAnimator;
        [SerializeField] private AnimationCurve _moveCurve;
        [SerializeField] private float _duration = 1f;

        private Camera _camera;
        private Coroutine _dragAnimation;
        private Transform _targetTransform;

        private void Awake()
        {
            _camera = Camera.main;
            Hide();
        }

        private void Update()
        {
            if(_targetTransform)
                transform.position = _targetTransform.position;
        }

        public void ShowClickAnimationIn(Transform point)
        {
            _targetTransform = point;
            _handPointerAnimator.ShowClickAnimation();
        } 

        public void ShowDragAnimation(Transform from, Transform to)
        {
            _targetTransform = null;
            _handPointerAnimator.ShowDragAnimation();
            _dragAnimation = StartCoroutine(DragShowing(from, to));
        }

        public void Show() =>
            gameObject.SetActive(true);

        public void Hide()
        {
            _targetTransform = null;
            StopDragAnimation();
            gameObject.SetActive(false);
        }

        private void StopDragAnimation()
        {
            if(_dragAnimation != null)
                StopCoroutine(_dragAnimation);
        }

        private IEnumerator DragShowing(Transform from, Transform to)
        {
            while (true)
            {
                Vector3 startPoint = _camera.WorldToScreenPoint(from.position);
                Vector3 endPoint = _camera.WorldToScreenPoint(to.position);
                
                _handPointerAnimator.ShowDragAnimation();
                for (float t = 0; t < 1; t += Time.deltaTime/ _duration)
                {
                    _rectTransform.anchoredPosition = Vector2.Lerp(startPoint, endPoint, _moveCurve.Evaluate(t));

                     yield return null;
                }

                yield return new WaitForSeconds(0.5f);
            }
        }
    }
}
