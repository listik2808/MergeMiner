using System.Collections;
using UnityEngine;

namespace Source.Scripts.UI
{
    public class BossGoalScreen : MonoBehaviour
    {
        [SerializeField] private Transform _panel;
        [SerializeField] private AnimationCurve _scaleCurve;
        [SerializeField] [Min(0)] private float _hideDuration = 0.4f;

        private Coroutine _hiding;
        
        private void Awake() =>
            gameObject.SetActive(false);

        private void OnDisable()
        {
            if(_hiding != null)
                StopCoroutine(_hiding);
        }

        public void Show() =>
            gameObject.SetActive(true);

        public void Hide()
        {
            if(isActiveAndEnabled)
                _hiding = StartCoroutine(Hiding());
        }

        private IEnumerator Hiding()
        {
            for (float t = 0; t < 1; t += Time.deltaTime/ _hideDuration)
            {
                _panel.localScale = Vector3.one * _scaleCurve.Evaluate(t);
                yield return null;
            }

            _panel.localScale = Vector3.zero;
            gameObject.SetActive(false);
        }
    }
}
