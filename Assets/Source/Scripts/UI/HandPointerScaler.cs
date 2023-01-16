using UnityEngine;
using UnityEngine.UI;

namespace Source.Scripts.UI
{
    public class HandPointerScaler : MonoBehaviour
    {
        [SerializeField] private RectTransform _rectTransform;
        [SerializeField] private CanvasScaler _canvasScaler;

        private void Update()
        {
            _rectTransform.localScale = Vector3.one * Screen.height / _canvasScaler.referenceResolution.y;
        }
    }
}
