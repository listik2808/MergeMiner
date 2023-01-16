using UnityEngine;

namespace Source.Scripts.UI.Tutorial
{
    public class TutorialCanvas : MonoBehaviour
    {
        [SerializeField] private HandPointer _handPointer;

        public HandPointer HandPointer => _handPointer;
    }
}
