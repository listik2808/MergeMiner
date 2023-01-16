using UnityEngine;

namespace Source.Scripts.Logic.GridBehaviour
{
    public class TrashZone : MonoBehaviour
    {
        [SerializeField] private BoxCollider _collider;

        private void Awake()
        {
            _collider.isTrigger = true;
        }
    }
}