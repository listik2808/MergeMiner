using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Source.Scripts.UI
{
    public class AdvBoxButton : MonoBehaviour
    {
        [SerializeField] private Button _button;
        [SerializeField] private AdvSlider _advSlider;
        [SerializeField] private ExtraRewardIcon _extraRewardIcon;
        [SerializeField] private AnimationCurve _animationCurve;
        [SerializeField] [Min(0)] private float _duration = 0.2f;

        private Coroutine _appearAnimation;
        
        public Button Button => _button;
        public AdvSlider AdvSlider => _advSlider;
        public ExtraRewardIcon ExtraRewardIcon => _extraRewardIcon;

        private void Awake()
        {
            gameObject.SetActive(false);
        }

        private void OnDisable()
        {
            if(_appearAnimation != null)
                StopCoroutine(_appearAnimation);
            transform.localScale = Vector3.zero;
        }

        public void Show()
        {
            gameObject.SetActive(true);
            _appearAnimation = StartCoroutine(AppearAnimation());
        }

        public void Hide()
        {
            if(isActiveAndEnabled)
                StartCoroutine(DisappearAnimation());
        }

        private IEnumerator AppearAnimation()
        {
            for (float t = 0; t < 1; t += Time.deltaTime / _duration)
            {
                transform.localScale = Vector3.one * _animationCurve.Evaluate(t);
                yield return null;
            }
        }

        private IEnumerator DisappearAnimation()
        {
            for (float t = 1; t > 0; t -= Time.deltaTime / _duration)
            {
                transform.localScale = Vector3.one * _animationCurve.Evaluate(t);
                yield return null;
            }
            gameObject.SetActive(false);
        }
    }
}
