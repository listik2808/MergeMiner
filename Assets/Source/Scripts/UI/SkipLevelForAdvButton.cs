using UnityEngine;
using UnityEngine.UI;

namespace Source.Scripts.UI
{
    public class SkipLevelForAdvButton : MonoBehaviour
    {
        [SerializeField] private Button _button;
        
        public Button Button => _button;
    }
}
