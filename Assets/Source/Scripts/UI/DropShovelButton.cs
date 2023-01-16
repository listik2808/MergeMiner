using UnityEngine;
using UnityEngine.UI;

namespace Source.Scripts.UI
{
    public class DropShovelButton : MonoBehaviour
    {
        [SerializeField] private Button _button;
        [SerializeField] private Transform _handPointerPosition;

        public Button Button => _button;
        public Transform HandPointerPosition => _handPointerPosition;
    }
}
