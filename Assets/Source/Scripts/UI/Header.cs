using TMPro;
using UnityEngine;

namespace Source.Scripts.UI
{
    public class Header : MonoBehaviour
    {
        [SerializeField] private TMP_Text _levelNumberText;

        public void SetLevelNumberText(int value)
        {
            _levelNumberText.text = value.ToString();
        }
    }
}