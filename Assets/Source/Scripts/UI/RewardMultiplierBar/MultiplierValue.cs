using DG.Tweening;
using TMPro;
using UnityEngine;

namespace Source.Scripts.UI.RewardMultiplierBar
{
    public class MultiplierValue : MonoBehaviour
    {
        [SerializeField] private int _value;
        [SerializeField] private TMP_Text _valueText;

        public int Value => _value;

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out MultiplierCursor cursor))
                _valueText.transform.DOScale(1.2f, 0.5f);
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.TryGetComponent(out MultiplierCursor cursor))
                _valueText.transform.DOScale(1f, 0f);
        }
    }
}
