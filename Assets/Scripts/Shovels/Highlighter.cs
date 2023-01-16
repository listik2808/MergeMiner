using UnityEngine;

namespace Shovels
{
    public class Highlighter : MonoBehaviour
    {
        [SerializeField] [Min(0)] private float _outlineWidth = 3.0f;
        [SerializeField] [Min(0)] private float _defaultOutlineWidth = 0.0f;
        [SerializeField] private Outline _outline;

        private void Awake() =>
            HighlightOff();

        public void HighlightOn() =>
            _outline.OutlineWidth = _outlineWidth;

        public void HighlightOff() =>
            _outline.OutlineWidth = _defaultOutlineWidth;
    }
}
