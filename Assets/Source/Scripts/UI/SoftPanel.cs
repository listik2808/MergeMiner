using UnityEngine;

namespace Source.Scripts.UI
{
    public class SoftPanel : MonoBehaviour
    {
        [SerializeField] private MoneyView _softText;
        
        public void SetSoftText(int value)
        {
            _softText.SetText(value);
        }
    }
}
