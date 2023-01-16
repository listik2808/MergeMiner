using System.Collections;
using TMPro;
using UnityEngine;

namespace Source.Scripts.UI
{
    public class DotsAnimation : MonoBehaviour
    {
        [SerializeField] private TMP_Text _dotsText;
        [SerializeField] [Min(0)] private float _dotAppearInterval = 0.5f;
        [SerializeField] [Min(1)] private int _dotsCount = 4;

        private IEnumerator Start()
        {
            var interval = new WaitForSeconds(_dotAppearInterval);
            
            while (true)
            {
                _dotsText.text = ".";
                for (int i = 0; i < _dotsCount; i++)
                {
                    yield return interval;
                    _dotsText.text += ".";
                }
            }
        }
    }
}
