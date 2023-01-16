using Source.Scripts.MergedObjects;
using UnityEngine;

namespace Source.Scripts.Logic.GoalChecker
{
    public class Chest : MonoBehaviour
    {
        public bool IsOpened { get; private set; }

        private void OnTriggerEnter(Collider other)
        {
            MergebleObject shovel = other.GetComponent<MergebleObject>();

            if (shovel)
                IsOpened = true;
        }
    }
}
